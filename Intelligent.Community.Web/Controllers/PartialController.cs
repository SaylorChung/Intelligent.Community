using Intelligent.Community.DataObjects;
using Intelligent.Community.DataObjects.Custom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;

namespace Intelligent.Community.Web.Controllers
{
    public class PartialController : CustomController
    {
        #region Private Properties
        private CustomMenu pageMenu;
        #endregion

        #region Public PartialView Mehods
        /// <summary>
        ///     左侧手风琴 分部页
        /// </summary>
        /// <returns></returns>
        public ActionResult Accordion()
        {
            StringBuilder sb = new StringBuilder();
            //当前登录用户ID
            Guid employeeId = FormsAuth.GetCookies<EmployeeDataObject>().ID;
            var response = AsyncGetContent("api/system/menu/" + employeeId + "/Home/Index").ReadAsAsync<ResponseModel>().Result;
            if (response.IsSuccess)
            {
                pageMenu = JsonConvert.DeserializeObject<CustomMenu>(response.Result.ToString());
                if (pageMenu != null)
                {
                    var empMenus = pageMenu.EmployeeMenus;
                    if (empMenus != null && empMenus.Count > 0)
                    {
                        //所有模块菜单集合
                        List<MenuDataObject> moduleResource = empMenus.FindAll(m => m.IsModule == true).OrderBy(m => m.Seq).ToList();
                        if (moduleResource != null)
                        {
                            //遍历所有模块
                            for (int i = 0; i < moduleResource.Count; i++)
                            {
                                //选中当前页面Accordion所属模块
                                sb.AppendFormat("<div title=\"{0}\" data-options=\"iconCls:'{1}'\">", moduleResource[i].Name, moduleResource[i].Code);
                                ///判断Module下是否存在子级节点
                                int count = empMenus.FindAll(m => m.IdParent == moduleResource[i].ID).Count;
                                if (count > 0)
                                {
                                    sb.AppendFormat("<ul class=\"easyui-tree home\" id=\"{0}\" style=\"margin:5px 0px 2px 0px;\">", moduleResource[i].ID);
                                    sb.AppendFormat(RecursiveChild(empMenus, moduleResource[i].ID));
                                    sb.AppendLine("</ul>");
                                }
                                sb.AppendLine("</div>");
                            }
                        }
                    }
                }
            }
            ViewBag.Accordion = sb.ToString();
            return PartialView();
        }

        /// <summary>
        ///     右侧PageHeader 分部页
        /// </summary>
        /// <returns></returns>
        public ActionResult PageHeader()
        {
            StringBuilder sb = new StringBuilder();
            //引用分部页的当前页Controller
            string controllerName = ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString();
            //引用分部页的当前页Action
            string actionName = ControllerContext.ParentActionViewContext.RouteData.Values["action"].ToString();
            if (controllerName == "Home" && actionName == "Index")
            {
                //首页
                sb.AppendLine("<h3 class=\"page-title\">控制台</h3>");
                sb.AppendLine("<ul class=\"breadcrumb\">");
                sb.AppendLine("<li><a href=\"/Home/Default\"><i class=\"fa fa-home\"></i>&nbsp;首页</a><span class=\"divider\">/</span></li><li class=\"active\">控制台</li>");
                sb.AppendLine("</ul>");
            }
            else
            {
                //当前登录用户ID
                Guid employeeId = FormsAuth.GetCookies<EmployeeDataObject>().ID;
                var response = AsyncGetContent("api/system/menu/" + employeeId + "/" + controllerName + "/" + actionName).ReadAsAsync<ResponseModel>().Result;
                if (response.IsSuccess)
                {
                    pageMenu = JsonConvert.DeserializeObject<CustomMenu>(response.Result.ToString());
                    if (pageMenu != null)
                    {
                        var menus = pageMenu.PageMenus;
                        if (menus != null && menus.Count > 0)
                        {
                            menus.Reverse();

                            sb.AppendFormat("<h3 class=\"page-title\">{0}</h3>", menus.LastOrDefault().Name);
                            sb.AppendLine("<ul class=\"breadcrumb\">");
                            for (int i = 0; i < menus.Count; i++)
                            {
                                if (i != menus.Count - 1)
                                    if (!string.IsNullOrEmpty(menus[i].LinkPath))
                                    {
                                        sb.AppendFormat("<li><a href=\"{0}\">{1}</a><span class=\"divider\">/</span></li>", menus[i].LinkPath, menus[i].Name);
                                    }
                                    else
                                    {
                                        if (menus[i].IsModule)
                                            sb.AppendFormat("<li><i class=\"{0}\"></i>&nbsp;{1}<span class=\"divider\">/</span></li>", menus[i].Code, menus[i].Name);
                                        else
                                            sb.AppendFormat("<li>{0}<span class=\"divider\">/</span></li>", menus[i].Name);
                                    }
                                else
                                    sb.AppendFormat("<li class=\"active\">{0}</li>", menus[i].Name);
                            }
                            sb.AppendLine("</ul>");
                        }
                    }
                }
            }
            ViewBag.PageHeader = sb.ToString();
            return PartialView();
        }
        #endregion

        #region Private Methods
        /// <summary>
        ///     递归获取子节点菜单。
        /// </summary>
        /// <param name="resourceList"></param>
        /// <param name="idParent"></param>
        /// <returns></returns>
        private string RecursiveChild(List<MenuDataObject> resourceList, Guid idParent)
        {
            List<MenuDataObject> list = resourceList.FindAll(m => m.IdParent == idParent).OrderBy(m => m.Seq).ToList();
            StringBuilder sb = new StringBuilder();
            if (list.Count > 0)
            {
                foreach (MenuDataObject entity in list)
                {
                    sb.AppendFormat("<li data-options=\"attribute:'{0}',id:'{1}'\" >", entity.LinkPath, entity.ID);
                    int count = resourceList.FindAll(m => m.IdParent == entity.ID).Count;
                    if (count > 0)
                    {
                        ///存在下级
                        sb.AppendFormat("<span>{0}</span>", entity.Name);
                        sb.AppendLine("<ul>");
                        sb.AppendFormat(RecursiveChild(resourceList, entity.ID));
                        sb.AppendLine("</ul>");
                    }
                    else
                    {
                        ///不存在下级
                        sb.AppendFormat("<span>{0}</span>", entity.Name);
                    }
                    sb.AppendLine("</li>");
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}