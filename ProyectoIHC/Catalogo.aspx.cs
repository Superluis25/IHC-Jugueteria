﻿using ProyectoIHC.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoIHC
{
    public partial class Catalogo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int pagina = 1;
                if(Request.QueryString["tipo"] != null)
                {
                    if (Request.QueryString["page"] != null)
                    {
                        pagina = int.Parse(Request.QueryString["page"].ToString());
                        if (pagina >= 1)
                        {
                            if (pagina >= 5)
                            {
                                hpvDes1.Enabled = false;
                                hpvDes2.Enabled = false;
                                hpvAnt1.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=4";
                                hpvAnt2.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=4";
                            }
                            if (pagina <= 1)
                            {
                                hpvAnt1.Enabled = false;
                                hpvAnt2.Enabled = false;
                                hpvDes1.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=2";
                                hpvDes2.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=2";
                            }
                            else
                            {
                                hpvAnt1.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=" + (pagina - 1);
                                hpvDes1.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=" + (pagina + 1);
                                hpvAnt2.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=" + (pagina - 1);
                                hpvDes2.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=" + (pagina + 1);
                            }
                        }
                    }
                    else
                    {
                        hpvAnt1.Enabled = false;
                        hpvAnt2.Enabled = false;
                        hpvDes1.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=2";
                        hpvDes2.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=2";
                    }
                    HyperLink2.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=1";
                    HyperLink3.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=2";
                    HyperLink4.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=3";
                    HyperLink5.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=4";
                    HyperLink6.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=5";

                    HyperLink7.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=1";
                    HyperLink8.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=2";
                    HyperLink9.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=3";
                    HyperLink10.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=4";
                    HyperLink11.NavigateUrl = HttpContext.Current.Request.Url.AbsolutePath + "?tipo=" + Request.QueryString["tipo"].ToString() + "&page=5";

                }
                

                using (DBManualConnection db = new DBManualConnection())
                {
                    String datos = Request.QueryString["tipo"].ToString();
                    String rango = "";
                    DataSet ds = db.getJuguetesRecomendados(datos, rango);
                    this.Session["DataSetJuguetes"] = ds;
                    if (ds.Tables[0].Rows.Count > (pagina - 1) * 15)
                    {
                        numArticulos.InnerText = "Resultado de " + ds.Tables[0].Rows.Count + " articulos";
                        repJueguetesListView.DataSource = ds;
                        repJueguetesListView.DataBind();

                        repJueguetesBlockView.DataSource = ds.Tables[0].Rows.Cast<System.Data.DataRow>().Skip((pagina - 1) * 15).Take(15).CopyToDataTable();
                        repJueguetesBlockView.DataBind();
                    }
                    else
                    {
                        numArticulos.InnerText = "Resultado de " + ds.Tables[0].Rows.Count + " articulos";
                        Hr1.Visible = true;
                        lblNoContent.Visible = true;
                        Hr2.Visible = true;
                    }

                }
                actualizarCarrito();
            }
        }

        protected void actualizarCarrito()
        {
            List<Producto> lst = this.Session["listaJuguetes"] as List<Producto>;
            if (lst.Count > 0)
            {
                carrito.Attributes.Add("class", carrito.Attributes["class"].ToString().Replace("empty", ""));
                conteo1.InnerHtml = lst.Count.ToString();
                conteo2.InnerHtml = (lst.Count + 1).ToString();
                int precioTotal = 0;
                foreach (Producto p in lst)
                {
                    String nombre = p.nombreProducto;
                    String precio = p.precio.ToString();
                    precioTotal += p.precio;
                    String id = p.id;
                    cuerpoCarrito.InnerHtml += "<li class='product'><div class='row'><div class='product-details'><h3><a>" + nombre + "</a></h3><span class='price'>$" + precio + "</span><div class='actions'><a data-id='" + id + "' data-name='" + nombre + "' data-price='" + precio + "' class='delete-item'>Delete</a></div></div></div></li>";
                }
                precioTotalSpan.InnerHtml = precioTotal.ToString();
            }
        }

        protected void repJueguetesListView_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = ((DataRowView)e.Item.DataItem);
                ((System.Web.UI.WebControls.Image)e.Item.FindControl("image1")).ImageUrl = "CargadorDeImagenes.aspx?ImageID=" + dr["id"];
            }
        }

        protected void repJueguetesBlockView_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = ((DataRowView)e.Item.DataItem);
                ((System.Web.UI.WebControls.Image)e.Item.FindControl("image2")).ImageUrl = "CargadorDeImagenes.aspx?ImageID=" + dr["id"];
            }
        }
    }
}