using AutoMapper;
using Intelligent.Community.DataObjects;
using Intelligent.Community.Domain.Enums;
using Intelligent.Community.Domain.Models;

namespace Intelligent.Community.Application
{
    /// <summary>
    ///     应用层服务初始化器。
    ///         1。AutoMapper框架的初始化
    /// </summary>
    public class ApplicationServiceInitailizer
    {
        public static void Initialize()
        {
          
            Mapper.CreateMap<Status, StatusDataObject>();
            Mapper.CreateMap<Employee, EmployeeDataObject>();
            Mapper.CreateMap<Menu, MenuDataObject>();

            Mapper.CreateMap<StatusDataObject, Status> ();
            Mapper.CreateMap<EmployeeDataObject, Employee>();
            Mapper.CreateMap<MenuDataObject, Menu>();
        }
    }
}
