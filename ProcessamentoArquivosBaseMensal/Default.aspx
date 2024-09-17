<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProcessamentoArquivosBaseMensal._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .invisivel {
            display: none;
        }


        .form-control {
            display: block;
            width: 100%;
            padding: 0.375rem 0.75rem;
            font-size: 1rem;
            font-weight: 400;
            line-height: 1.5;
            color: #212529;
            background-color: rgba(0, 0, 0, 0.075);
            background-clip: padding-box;
            border: 1px solid #dc3545;
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            border-radius: 0.375rem;
            transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
        }

        
    </style>
    <div class="jumbotron">
        
        <div style="display: flex; align-items: center;">
            <p>
                <asp:Button runat="server" ID="btImportar" title="Importar arquivo" OnClick="ImportarArquivo" type="submit" Text="Importar" class="btn btn-warning btn-md botao-salvar"></asp:Button>
            </p>

            <div class="input-group mensagemAlerta invisivel" id="grupoMensagemAlerta" runat="server" style="margin-left:10px;" >
                <input type="text" class="form-control" id="mensagemAlerta" runat="server" />
            </div>
        </div>
    </div>

</asp:Content>
