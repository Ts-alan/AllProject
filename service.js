var imageWidth, imageHeight;
var divtag, imgtag;
var posimg;

function initDHTMLAPI(divid, imgid, pos) {
    if (document.images) {
        isCSS = (document.body && document.body.style) ? true : false;
        isW3C = (isCSS && document.getElementById) ? true : false;
        isIE4 = (isCSS && document.all) ? true : false;
        isNN4 = (document.layers) ? true : false;
        isIE6CSS = (document.compatMode && document.compatMode.indexOf("CSS1") >= 0) ? true : false;
		/*alert("isCSS" + isCSS);
		alert("isW3C" + isW3C);
		alert("isIE4" + isIE4);
		alert("isNN4" + isNN4);
		alert("isIE6CSS" + isIE6CSS);*/
    }
    
    divtag = divid;
    imgtag = imgid;
    
    posimg = pos;
    
    obj = document.getElementById(imgid);
    imageWidth = obj.width;
    imageHeight = obj.height;
}
function getInsideWindowWidth() {
    if (window.innerWidth) {
        return window.innerWidth;
    } else if (isIE6CSS) {
        return document.body.parentElement.clientWidth;
    } else if (document.body && document.body.clientWidth) {
        return document.body.clientWidth;
    }
    return 0;
}
function getInsideWindowHeight() {
    if (window.innerHeight) 
	{
		return window.innerHeight;
    } 
	else if (isIE6CSS) 
	{
		return document.body.parentElement.clientHeight;
    } 
	else if (document.body && document.body.clientHeight) 
	{
		return document.body.clientHeight;
    }
    return 0;
}
function TilingBG()
{
    var obj = document.getElementById(imgtag);
    var BaseObj = document.getElementById(divtag);
    var idNom = 1;
    var id = imgtag + "_";
    var newImg;
    while(true)
    {
        newImg = document.getElementById((id + idNom));
        if( newImg == null ) break;
        BaseObj.removeChild(newImg);
        idNom +=1;        
    }
    
    var inner_w = getInsideWindowWidth();
    var inner_h = getInsideWindowHeight();
    
    var x = (inner_h*imageWidth/imageHeight);
    var y = (inner_w*imageHeight/imageWidth);

    idNom = 1;
    var offset = 0;
    if( inner_w > inner_h )
    {
        obj.style.left = "0px";
        obj.style.height = inner_h+"px";
        obj.style.width = x+"px";
        delta = inner_w - x;
        offset = x;
    }
    else
    {
        obj.style.top = "0px";
        obj.style.width = inner_w+"px";
        obj.style.height = y+"px";
        delta = inner_h - y;
        offset = y;
    }

    while( delta > 0 )
    {
        newImg = obj.cloneNode(false); 
        newImg.id = id + idNom;
        newImg.style.position = "absolute";

        if( inner_w > inner_h )
        {
            newImg.style.left = offset+"px";
            offset += x;
            delta -= x;
        }
        else
        {
            newImg.style.top = offset+"px";
            offset += y;
            delta -= y;
        }

        BaseObj.appendChild(newImg);
        idNom++;
    }
}
function CenterZoomBG()
{
    var obj = document.getElementById(divtag);
    if( !obj ) return;
    
    obj = document.getElementById(imgtag);
    
    if( posimg == "center" )
    {
        var inner_w = getInsideWindowWidth();
        var inner_h = getInsideWindowHeight();
        
        if( inner_w > inner_h )
        {
            obj.style.top="0px";
            obj.style.height = inner_h+"px";
            var x = (inner_h*imageWidth/imageHeight);
            obj.style.width = x+"px";
            obj.style.left = ((inner_w - x)/2)+"px";
        }
        else
        {
            obj.style.left = "0px";
            obj.style.width = inner_w+"px";
            var y = (inner_w*imageHeight/imageWidth);
            obj.style.height = y+"px";
            obj.style.top = ((inner_h - y)/2)+"px";
        }
    }
    else TilingBG();
}
