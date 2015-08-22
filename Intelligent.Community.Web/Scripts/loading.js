//获取浏览器页面可见高度和宽度
var _PageHeight = document.documentElement.clientHeight,
    _PageWidth = document.documentElement.clientWidth;
//计算loading框距离顶部和左部的距离（loading框的宽度为92，高度为31px）
var _LoadingTop = _PageHeight > 31 ? (_PageHeight - 31 - 40) / 2 : 0,
    _LoadingLeft = _PageWidth > 92 ? (_PageWidth - 92 - 100) / 2 : 0;
//在页面未加载完毕之前显示的loading Html自定义内容
var _LoadingHtml = '<div id="loadingDiv" style="position:absolute;left:0;width:100%;height:' + _PageHeight + 'px;top:0;background:#fff;z-index:10000;"><div style="position: absolute; left: ' + _LoadingLeft + 'px; top:' + _LoadingTop + 'px;line-height: 40px; padding-left: 30px;background: #fff url(/images/loading.gif) no-repeat scroll 5px 10px;color: #696969;">loading...</div></div>';
//呈现loading效果
document.write(_LoadingHtml);
//监听加载状态改变
document.onreadystatechange = completeLoading;
//加载状态为complete时移除loading效果
function completeLoading() {
    if (document.readyState == "complete") {
        var loadingMask = document.getElementById('loadingDiv');
        loadingMask.parentNode.removeChild(loadingMask);
    }
}