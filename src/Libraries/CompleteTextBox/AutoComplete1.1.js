
              var obj;
              function getXMLHTTPRequest() 
              {
                 var xRequest=null;
                if (window.XMLHttpRequest) {
                xRequest=new XMLHttpRequest();
                }else if (typeof ActiveXObject != "undefined"){
                  xRequest=new ActiveXObject("Microsoft.XMLHTTP");
                }
                  return xRequest;
                }
            function GetDataViaAJAX(prefix,WhoGet)
            {
                obj=getXMLHTTPRequest();
                if(obj!=null)
                {   obj.onreadystatechange =function() {ReceiveServerData(WhoGet)};
                    obj.open("GET","DropDownListHandler.ashx?prefix="+escape(prefix) ,  true);//урла страницы данные с которой будет читать
                    obj.send(null);         
                }
                return false;
            }
       
        function TextKeyDown(WhoSend,WhoGet,eventX) //Отправка данных на сервер и проверка нажатых клавиш в текстовом поле
        {
            if(eventX.keyCode==13)
            {
                return false;
            }
            else if(eventX.keyCode==27)
            {
                document.getElementById(WhoGet).style.display='none'
                return false;
            }
            return true;
        }
        function SendData(WhoSend,WhoGet,eventX) //Отправка данных на сервер и проверка нажатых клавиш в текстовом поле
        {
            var objGet=document.getElementById(WhoGet);
            if (WhoSend.value.length<2){objGet.style.display='none';return false;}//Ничего не делать, если введено меньше 2-х символов
            var e=eventX;
            if (e.keyCode==40) //Нажата стрелка вниз
            {
                if (objGet.selectedIndex<objGet.options.length-1)
                    objGet.selectedIndex++;
                else 
                    objGet.selectedIndex=0;
                return false;
            }else if (e.keyCode==38) //Нажата стрелка вверх
            {
                if (objGet.selectedIndex>0)
                    objGet.selectedIndex--;
                else 
                    objGet.selectedIndex=objGet.options.length-1;
                return false;
            }else if (e.keyCode==13) //Нажат ввод
            {
                ReturnValueToTextBoxs(objGet,WhoSend)
                return false;
            }else if (e.keyCode==27) //Нажат Escape
            {
                objGet.style.display='none';
                return false;
            }
            //Выполнение запроса на сервер и отображение списка подстановки
            var offset=GetLocation(WhoSend) //Получение координат текстового поля
            GetDataViaAJAX(WhoSend.value,WhoGet);
            objGet.style.top =offset.y+WhoSend.offsetHeight+'px';
            objGet.style.left =offset.x+'px';
            objGet.style.width =offset.width+'px';
            objGet.style.display='block'; //Отображение списка на экране
        }
        function GetLocation(element)//Определение координат объекта
        {
            var offsetX =0;
            var offsetY =0;
            var widthElt =element.offsetWidth;
            var heightElt =element.offsetHeight;
            var parent;
            for (parent =element;parent;parent =parent.offsetParent)
            {
                if (parent.offsetLeft)
                {
                    offsetX +=parent.offsetLeft;
                }
                if (parent.offsetTop)
                {
                    offsetY +=parent.offsetTop;
                }
            }
            return{x:offsetX,y:offsetY,width:widthElt,height:heightElt};
        }

        function ReceiveServerData(context) //Поучение данных с сервера
        {   
        
            var args='';
                if(obj.readyState == 4) 
                {
                    if(obj.status == 200)
                    {
                        args=obj.responseText;  
                    }
                    else
                    {
                        alert("Error retrieving data!" );
                    }   
                }
                
            var select=document.getElementById(context)
            if (args.length=0){return false;} //Ничего не делаем, если ничего не получено
           
            var j=0;
            var mass=args.split("\t"); //Превращаем строку в массив
            for (var i=select.length-1;i>-1;i--) //Очищаем список
                select.options[i]=null;
            for (var i=0;i<mass.length-1;i++)
            {
                select.options[j]=new Option(mass[i]); //Заполняем список
                j++
            }
             
            if (j!=0)
                select.size=j+1; //Выставляем размер с списка
            else
                            select.size=2;

        }        
        function ReturnValueToTextBoxs(SelectObj,TextObj)//Заполняем текстовое поле
        {
            if (SelectObj.selectedIndex!=-1){
            TextObj.value=SelectObj.options[SelectObj.selectedIndex].text;
            //IDTextObj.value=SelectObj.options[SelectObj.selectedIndex].value;
            TextObj.focus();
            SelectObj.style.display='none';
            }
        }
        function SelectItem(select,textbox,mouse,EventX) //Обрабатываем события списка подстановки
        {
            alert('s');
            TextObj=document.getElementById(textbox);
            //IDTextObj=document.getElementById(idtextbox);
            SelectObj=select;
            if (mouse==true)
            {
                ReturnValueToTextBoxs(SelectObj,TextObj);
            }
            else
            {
                var e=EventX;
                if (e.keyCode==13)
                {
                    ReturnValueToTextBoxs(SelectObj,TextObj);
                 
                }
                else if(e.keyCode==27)
                {
                    TextObj.focus();
                    SelectObj.style.display='none';
                }
                
            }
            e.returnValue=false;
        }
