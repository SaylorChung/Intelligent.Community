/*
    WebApi请求前缀
*/
var apiPrefix = "http://localhost:1221/";
/*
    datagrid 配置
*/
var grid = {
    url: "",
    method: "GET",
    idField: "ID",
    singleSelect: true,
    rownumbers: true,
    checkOnSelect: false,
    selectOnCheck: false,
    sortName: 'ID',
    sortOrder: 'desc',
    nowrap: false,
    fitColumns: true,
    columns: [],
    checkbox: false,
    queryParams: {},
    pagination: true,
    rownumbers: true,
    pageNumber: 1,
    pageSize: 10,
    pageList: [10, 20, 50],
    loadFilter: function (data) {
        if (data.IsSuccess)
            return data.Result;
        else
            messageBox.warning(data.Result, function () { });
        return { "total": 0, "rows": [] }
    }
};
/*
    弹窗对象
*/
var messageBox = {};
/*
    默认弹窗
*/
messageBox.alert = function (message) {
    TopWin().Msgslide(message);
};
/*
    信息弹窗
*/
messageBox.info = function (message, callback) {
    $.messager.alert("信息", message, 'info', callback);
};
/*
    警告弹窗
*/
messageBox.warning = function (message) {
    $.messager.alert('警告', message, 'warning');
};
/*
    错误弹窗
*/
messageBox.error = function (message) {
    $.messager.alert('错误', message, 'error');
};
/*
    询问弹窗
*/
messageBox.confirm = function (message, callback) {

    $.messager.confirm('询问', message, callback);
};
/*
    表单弹窗
*/
messageBox.window = function (title, url,callback, isModal) {
    var content = '<iframe scrolling="no" frameborder="0"  src="' + url + '" style="width:100%;height:99%;overflow: hidden;"></iframe>';
    $('#ContentWindow').window({
        title: title,
        content: content,
        modal: isModal,
        minimizable: false,
        maximizable: false,
        maximized:true,
        shadow: true,
        cache: false,
        closed: false,
        collapsible: false,
        resizable: false,
        draggable:false,
        loadingMessage: '正在加载，请稍等片刻......',
        onClose: callback
    });
};
/*
    关闭弹窗
*/
function CloseWin() {
    try {
        parent.$('#ContentWindow').window('close');
    } catch (e) { }
}
/*
    获取最底级窗体
*/
function TopWin() {
    var currentWin = window;
    while (currentWin.parent) {
        currentWin = currentWin.parent;
        if (currentWin.self == window.top) {
            break;
        }
    }
    return currentWin;
}
/*
    禁用ajax缓存，
        添加ajax异常处理
*/
$.ajaxSetup({
    cache: false,
    beforeSend: function (req) {
        //req.setRequestHeader('Authorization', 'saylor c2F5bG9y');
    },
    error: function (data) {
        messageBox.error(data.responseJSON.Message);
    }
});
/*
    页面操作
*/
var win = {};
/*
    删除数据
*/
win.Delete = function (url) {
    var rows = $("#grid").datagrid("getSelections");
    if (rows.length == 0) {
        messageBox.warning("请选中一行记录！");
        return;
    }
    else if (rows.length > 1) {
        messageBox.warning("只能选择一条记录！");
        return;
    }
    messageBox.confirm("确认删除选中信息?", function (isConfirm) {
        if (isConfirm) {
            $.ajax({
                type: "DELETE",
                url: url + "/" + rows[0].ID,
                async: false,
                success: function (data) {
                    messageBox.alert(data.Result);
                    $('#grid').datagrid('clearSelections');
                    $('#grid').datagrid("reload");
                }
            });
        }
    });
};
/*
    编辑弹窗
*/
win.Edit = function (title, url, callback, ismodal) {
    var rows = $("#grid").datagrid("getSelections");
    if (rows.length == 0) {
        messageBox.alert("请选中一行记录！");
        return;
    }
    else if (rows.length > 1) {
        messageBox.alert("只能选择一条记录！");
        return;
    }
    url = url + "/" + rows[0].ID;
    messageBox.window(title, url, callback, true);
};
/*
    新增弹窗
*/
win.Add = function (title, url, callback, ismodal) {
    messageBox.window(title, url, callback, true);
};
/*
    初始化
*/
$(function () {
    if (TopWin().Msgslide == undefined) {
        alert("严禁盗链打开页面，准备跳转到主页面。。。");
        window.location.href = "/";
    }
    /*
        niceScroll
    */
    $("body").niceScroll({ styler: "fb", cursorcolor: "#FF2D2D", cursorwidth: '4', cursorborderradius: '0px', background: '#F0FFF0', cursorborder: '', zindex: '1000' });
    /*
        widget tools
            fa-chevron-down
    */
    $('.widget .tools .fa-chevron-down').click(function () {
        var el = $(this).parents(".widget").children(".widget-body");
        if ($(this).hasClass("fa fa-chevron-down")) {
            $(this).removeClass("fa fa-chevron-down").addClass("fa fa-chevron-up");
            el.slideUp(200);
        } else {
            $(this).removeClass("fa fa-chevron-up").addClass("fa fa-chevron-down");
            el.slideDown(200);
        }
    });
    /*
        widget tools
            fa-remove
    */
    $('.widget .tools .fa-remove').click(function () {
        $(this).parents(".widget").parent().remove();
    });
    /*
        fa-bars
    */
    $('.fa-bars').click(function () {
        TopWin().$('.fa-bars').click();
    });
});