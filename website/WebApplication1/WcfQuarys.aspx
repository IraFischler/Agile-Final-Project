<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="WcfQuarys.aspx.cs" Inherits="WebApplication1.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <asp:Panel runat="server" ID="Panel2">
    <form id="form1" runat="server">
    <div>
    
        <i><asp:Button ID="DisplayPlayers" runat="server" OnClick="DisplayAllPlayers" Text="display all players" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </i>
        </div>
        <p>
                <asp:Button ID="Button1" runat="server" OnClick="DisplayAllGames" Text="display all games" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </p>
        <p>
                <asp:Button ID="Button3" runat="server" OnClick="Button1_Click" Text="How many games did a player make" />
                </p>
        <p>
                choose a player&nbsp;&nbsp;
                <asp:DropDownList ID="DropDownListPlayers" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListPlayers_SelectedIndexChanged" style="height: 22px">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:Button ID="DeletePlayer" runat="server" OnClick="DeletePlayer_Click" Text="Delete Player" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="UpdatePlayer" runat="server" Text="Update" OnClick="UpdatePlayer_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Name: "></asp:Label>
                <asp:TextBox ID="UpdateName" runat="server" ></asp:TextBox>
                &nbsp;
                <asp:Label ID="Label2" runat="server" Text="Email: " ></asp:Label>
                &nbsp;<asp:TextBox ID="UpdateEmail" runat="server" ></asp:TextBox>
                </p>
        <p>
                choose a game&nbsp;&nbsp;
                <asp:DropDownList ID="DropDownListGames" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="DropDownListGames_SelectedIndexChanged" style="height: 22px">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:Button ID="DeleteGame" runat="server" OnClick="DeleteGame_Click" Text="Delete Game" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </p>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <br />
        &nbsp;<asp:Panel ID="Panel4" runat="server">
            <a href="About.aspx">Abaut</a>
            <a href="RegisterForm.aspx">RegisterForm</a>
        </asp:Panel>
        <br />
        <br />
    </form>
        </asp:Panel>
</body>
</html>
