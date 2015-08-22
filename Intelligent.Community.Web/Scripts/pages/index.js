$(function () {
    /*
        niceScroll
    */
    $("#Content").niceScroll({ styler: "fb", cursorcolor: "#FF2D2D", cursorwidth: '4', cursorborderradius: '0px', background: '#F0FFF0', cursorborder: '', zindex: '1000' });
    $("#leftContent").niceScroll({ styler: "fb", cursorcolor: "#FF2D2D", cursorwidth: '2', cursorborderradius: '0px', background: '#F0FFF0', cursorborder: '', zindex: '1000' });
    /*
        Accordion tree
    */
    $("[class='panel layout-panel layout-panel-west']").find(".panel-header").eq(0).css({ "height": "40px", "background-color":"rgba(255, 215, 168, 1)","font-size":"14px" });
    $("[class='panel layout-panel layout-panel-west']").find(".panel-header").find("div").eq(0).css("color", "black")
    $("[class='panel layout-panel layout-panel-west']").find("[class='panel-title panel-with-icon']").eq(0).css({ "margin-top": "13px", "font-size": "14px" })
    $("[class='easyui-tree home tree']").find(".tree-icon.tree-file").parent().parent().addClass("treeline");
    /*
        easyui-tree
    */
    $("[class='easyui-tree home tree']").tree({
        animate: true,
        onClick: function (node) {
            if (node.attribute) {
                $("[class='easyui-tree home tree']").find("li > ul > li").removeClass("tree-node-selected");
                $("[class='easyui-tree home tree']").tree("select", node.target);
                $("#pageContent").attr("src", node.attribute);
            }
        }
    });
    /*
        fa-bars
    */
    $('.fa-bars').click(function () {
        if ($("[class='panel layout-panel layout-panel-west']").css('display') != 'block') {
            $(".layout-button-right").click();
        }
        else {
            $(".layout-button-left").click();
        }
    });
    $("#pageContent").attr("src", "/Home/Default");
});
/*
    弹窗配置
*/
function Msgslide(msg) {
    $.messager.show({
        title: "提示",
        msg: msg,
        timeout: 3000,
        showType: 'slide'
    });
}
