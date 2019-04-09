<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASP_presentation._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jquery.ui/1.10.3/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.10.3/themes/sunny/jquery-ui.css">
    
    <style>
        #DivPreview
        {
            width: 1200px;
        }
    </style>


    <div id="Main_div">
        <table id="Main_table">
            <tr id="Main_table_Rows">
                <th>
                    <center>
                        <button type="button" ID="AddTextButton">Добавить текст</button>
                        <input type="text" style="width: 500px" ID="TextBoxText"/>
                        <input id="white" name="color" type="radio" value="white" checked> Белый текст
                        <input id="black" name="color" type="radio" value="black"> Черный текст
                    </center>
                </th>
                <th>
                    <center>Выбор фона</center>
                </th>
            </tr>
            <tr>
                <td id="Main_table_Main" runat="server">
                    <div id="DivPreview">
                        <asp:Image ID="Image_Preview" runat="server" />
                    </div>
                </td>
                <td id="Main_table_Choice">
                    <asp:Panel class="Main_Panels" runat="server" ScrollBars="Vertical" BorderStyle="None" Height="800">
                        <asp:Repeater ID="RepeaterImages" runat="server">
                            <ItemTemplate>
                                <asp:ImageButton 
                                    ID="ImageBtn" 
                                    runat="server" 
                                    class="Main_image" 
                                    ImageUrl='<%# Eval("Name") %>'
                                    OnClick="Image_Click"
                                    CommandArgument='<%# Eval("Name") %>' />
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <br/>
        <br/>
        <asp:FileUpload ID="FileUploadControl" runat="server" AllowMultiple="True" />
        <asp:Button ID="UploadButton" runat="server" OnClick="UploadButton_Click" Text="Загрузить картинки" />
        <asp:Button ID="ClearButton" runat="server" OnClick="ClearButton_Click" Text="Очистить историю" />
        <br/>
        <asp:Label runat="server" ID="StatusUpload" Text="" />
        <asp:Button ID="DownloadButton" runat="server" Text="Скачать слайд" OnClick="DownloadButton_Click" />
        <br/>
        <asp:Label runat="server" ID="LabelDownload" Text=""/>
        <asp:HiddenField id="colors" runat="server"></asp:HiddenField>
        <asp:HiddenField id="textsArr" runat="server"></asp:HiddenField>
        <asp:HiddenField id="posX" runat="server"></asp:HiddenField>
        <asp:HiddenField id="posY" runat="server"></asp:HiddenField>
    </div>

    <script type="text/javascript">
        var arr = [];
        var length = arr.length;
        var textArr = [];
        var xPosition = [];
        var yPosition = [];
        var colors = [];

        document.getElementById("AddTextButton").onclick = function () {
            var text = document.getElementById('TextBoxText');
            length = length + 1;

            if (text.value != "") {
                var div = document.createElement('div');
                div.id = "dragElement" + length;
                div.style.position = "absolute";
                div.style.width = "auto";
                div.style.fontSize = "x-large";
                div.style.padding = "10px";
                div.innerHTML = text.value;
                div.style.cursor = "move";

                if (document.getElementById('white').checked) {
                    div.style.color = "white";
                    colors.push("white");
                }
                else {
                    div.style.color = "black";
                    colors.push("black");
                }

                arr.push('#' + div.id);
                textArr.push(div.innerHTML);

                var parent = document.getElementById("DivPreview");
                
                parent.insertBefore(div, parent.firstChild);

                document.getElementById('<%= colors.ClientID %>').value = colors;
                document.getElementById('<%= textsArr.ClientID %>').value = textArr;

                text.value = "";
            }
            else {
                alert("Введите текст!");
            }

            arr.forEach(function(currentValue, index) {
                $(function () {
                    $(currentValue).draggable({
                        containment: "parent",
                        grid: [1, 1],
                        drag: function () {
                            var offset = $(this).offset();
                            xPosition[index] = offset.left;
                            yPosition[index] = offset.top;
                            document.getElementById('<%= posX.ClientID %>').value = xPosition;
                            document.getElementById('<%= posY.ClientID %>').value = yPosition;
                        },
                    })
                });
            });
        };
    </script>
</asp:Content>



