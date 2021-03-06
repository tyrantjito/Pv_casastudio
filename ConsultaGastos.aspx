﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="ConsultaGastos.aspx.cs" Inherits="ConsultaGastos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <center>
        <div class="ancho95 centrado center">
            <div class="row">
                <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                   <i class="icon-th-list"></i> <asp:Label runat="server" ID="lblTitulo" Text="Gastos" CssClass="alert-success"></asp:Label>
                </div>
            </div>
            <div class="col-lg-12 col-sm-12">                
                        <div class="col-lg-3 col-sm-3">
                            <asp:Label ID="Label1" runat="server" Text="Inicio:"></asp:Label>
                            <asp:TextBox ID="txtFechaIni" runat="server" CssClass="input-small" MaxLength="10" Enabled="false"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" PopupButtonID="lnkFini"
                                BehaviorID="txtFechaIni_CalendarExtender" TargetControlID="txtFechaIni" Format="yyyy-MM-dd">
                            </cc1:CalendarExtender>
                            <asp:LinkButton ID="lnkFini" runat="server" CssClass="btn btn-info"><i class="icon-calendar"></i></asp:LinkButton>
                        </div>
                        <div class="col-lg-2 col-sm-3">
                            <asp:Label ID="Label2" runat="server" Text="Fin:"></asp:Label>
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="input-small" MaxLength="10" Enabled="false"></asp:TextBox>                        
                            <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" PopupButtonID="lnkFFin"
                                BehaviorID="txtFechaFin_CalendarExtender" TargetControlID="txtFechaFin" Format="yyyy-MM-dd">
                            </cc1:CalendarExtender>
                            <asp:LinkButton ID="lnkFFin" runat="server" CssClass="btn btn-info"><i class="icon-calendar"></i></asp:LinkButton>
                        </div>
                        <div class="col-lg-3 col-sm-3">
                            <asp:Label ID="Label7" runat="server" Text="Tienda:"></asp:Label>
                            <asp:DropDownList ID="ddlIslas" runat="server" CssClass="input-medium" AppendDataBoundItems="true"
                                DataSourceID="SqlDataSource2" DataTextField="nombre" DataValueField="idAlmacen">
                                <asp:ListItem Selected="True" Text="Todos" Value="T" />
                            </asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select u.id_punto as idAlmacen,p.nombre_pv as nombre from usuario_puntoventa U inner join punto_venta p on p.id_punto=u.id_punto where U.usuario=@usuario and U.estatus='A'">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="usuario" QueryStringField="u" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <div class="col-lg-3 col-sm-3">
                            <asp:Label ID="Label3" runat="server" Text="Usuario:" />
                            <asp:DropDownList ID="ddlUsuario" runat="server" AppendDataBoundItems="true" DataSourceID="SqlDataSource1" 
                                CssClass="input-medium" DataTextField="nombre" DataValueField="usuario" >
                                <asp:ListItem Text="Todos" Selected="True" Value="T"  />
                            </asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                                SelectCommand="select usuario,(nombre+' '+apellido_pat+' '+isnull(apellido_mat,'')) as nombre from usuarios_PV where perfil=2 and estatus='A'"></asp:SqlDataSource>
                        </div>
                
                        <div class="col-lg-1 col-sm-1"><asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn-info" ValidationGroup="busca" OnClick="btnBuscar_Click" /></div>
                    
            </div>                    
            <div class="col-lg-12 col-sm-12 center alert-danger">
                <asp:Label ID="lblError" runat="server" CssClass="errores negritas"></asp:Label>
            </div>
            <div class="col-lg-12 col-sm-12 center marTop">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:GridView ID="GridGastos" EmptyDataText="No se han registrado gastos." runat="server" 
                            CssClass="table table-bordered center" AutoGenerateColumns="false" 
                            PageSize="7" AllowPaging="true" OnPageIndexChanging="GridGastos_PageIndexChanging" OnRowDataBound="GridGastos_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Fecha" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label41" runat="server" Text='<%# Eval("fecha", "{0:yyyy-MM-dd}") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Hora" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label42" runat="server" Text='<%# Eval("hora") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tienda" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label43" runat="server" Text='<%# Eval("nombre") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Usuario" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label44" runat="server" Text='<%# Eval("nombreU") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Referencia" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label45" runat="server" Text='<%# Eval("referencia") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gasto" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label46" runat="server" Text='<%# Eval("justificacion") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Monto" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblImporte" runat="server" Text='<%# Eval("importe") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle CssClass="errores" />
                        </asp:GridView>
                        <div class="col-lg-12 col-sm-12 text-right negritas marTop">
                            <asp:Label runat="server" ID="lblTotal" CssClass="negritas" Text="Total:" />&nbsp;
                            <asp:Label runat="server" ID="lblMontoTotal" CssClass="negritas" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </center>
</asp:Content>

