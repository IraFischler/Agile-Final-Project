<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterForm.aspx.cs" Inherits="WebApplication1.RegisterForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .mytd1 {
            width: 200px;
        }

        .mytd2 {
            width: 100px;
        }
        </style>
</head>
<body>
    <asp:Panel runat="server" ID="Panel1">
        <h2>This is a registration form</h2>
        <form id="form1" runat="server" target="_self">
            <p>
                Register for Game <i>&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                </i>
            </p>
            <div>
                <table>
                    <tr>
                        <td class="mytd2">Name:</td>
                        <td class="mytd1">
                            <asp:TextBox ID="TextBoxName" runat="server"  />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxName" Display="Dynamic" ErrorMessage="A Name must be enterd" ValidationGroup="AllValidators">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TextBoxName" Display="Dynamic" ErrorMessage="invalid name" ValidationExpression="^[a-zA-Z'\s]{1,40}$" ValidationGroup="AllValidators">*</asp:RegularExpressionValidator>
                        </td>                        
                        <td>E-mail:</td>
                        <td>
                            <asp:TextBox ID="TextBoxEmail" runat="server"  style="height: 22px" />
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxEmail" Display="Dynamic" ErrorMessage="Email address is required	" ValidationGroup="AllValidators">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxEmail" Display="Dynamic" ErrorMessage="E-mail addresses must be in the format of name@domain.xyz." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="AllValidators">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Register" ValidationGroup="AllValidators" />
                <br />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Num of Players"></asp:Label>
                &nbsp;<asp:TextBox ID="TextBox1" runat="server"  Width="62px"  >
                      </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="TextBox1" Display="Dynamic" ErrorMessage="Num of player must be a number" ValidationExpression="\d" ValidationGroup="AllValidators">*</asp:RegularExpressionValidator>
                     
                &nbsp;&nbsp;&nbsp;
                
                <asp:Button ID="TeamGameButton" runat="server" OnClick="TeamGameButton_Click" Text="Register to team" />
                
            </div>

            <p style="direction: rtl">
                &nbsp;
            </p>
            <asp:Panel ID="Panel2" runat="server">
                <p>
                    Register for Team, write above the name and email of the player, they will bee added to the team.
                </p>
                <p>
                    Team name:&nbsp;
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                   <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="TextBox2" Display="Dynamic" ErrorMessage="Invalid Team Name" ValidationExpression="^[a-zA-Z'\s]{1,40}$" ValidationGroup="AllValidators">*</asp:RegularExpressionValidator>
                        
                    &nbsp;&nbsp;
                    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Resgister Team" />
                </p>                
            </asp:Panel>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AllValidators" DisplayMode="BulletList" EnableClientScript="True" />
        </form>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server">
            <a href="About.aspx">Abaut</a>
            <a href="WcfQuarys.aspx">WcfQuarys</a>
        </asp:Panel>

</body>
</html>