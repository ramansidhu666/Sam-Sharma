﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Property_cls;
using System.Data.SqlClient;


namespace Property
{
    public partial class Property_New1 : System.Web.UI.MasterPage
    {
        #region Global

        cls_Property clsobj = new cls_Property();

        #endregion Global
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMenusList();
                SiteSetting();
               
            }
            if (Request.QueryString["MLSID"] != null && Request.QueryString["PropertyType"] != null)
            {
                SetValuesFaceBookShare();
            }
        }

        private void SetValuesFaceBookShare()
        {
            String MLSID = Request.QueryString["MLSID"];
            String PropertyType = Request.QueryString["PropertyType"];
            fbProprtyImage.Content = "";
            fbProprtyTitle.Content = "";
            fbProprtyUrl.Content = "";
            //fbProprtySiteName.Content = "";
            fbProprtyShareType.Content = "";

            Property1.MLSDataWebServiceSoapClient mlsClient = new Property1.MLSDataWebServiceSoapClient();

            DataTable dt = new DataTable();
            if (PropertyType.Contains("Residential"))
            {
                dt = mlsClient.GetResidentialProperties(Convert.ToString(Request.QueryString["MLSID"]), "0", "0", "0", "0", "0", "0");
            }
            else if (PropertyType.Contains("Commercial"))
            {
                dt = mlsClient.GetAllCommercialProperties(Request.QueryString["MLSID"].ToString(), "0", "0", "0", "0", "0");
            }
            else if (PropertyType.Contains("Condo"))
            {
                dt = mlsClient.GetProperties_Condo(Convert.ToString(Request.QueryString["MLSID"]), "0", "0", "0", "0", "0", "0");
            }


            if (dt.Rows.Count > 0)
            {
                //dt.Rows[0][]
                fbProprtyImage.Content = dt.Rows[0]["pImage"].ToString();
                fbProprtyTitle.Content = string.Format("{0}, {1}, {2}, {3}", dt.Rows[0]["Address"].ToString(), dt.Rows[0]["Municipality"].ToString(), dt.Rows[0]["PostalCode"].ToString(), dt.Rows[0]["province"].ToString());
                fbProprtyUrl.Content = string.Format("http://gtadreamhome.ca/PropertyDetails.aspx?MLSID={0}&PropertyType={1}", dt.Rows[0]["MLS"].ToString(), dt.Rows[0]["pType"].ToString());
                //fbProprtySiteName.Content = "";
                fbProprtyShareType.Content = "Property";


            }


        }

        private void BindMenusList()
        {
            StringBuilder StrMenu = new StringBuilder();
            DataTable dt = new DataTable();
            DataTable dtSubmenu = new DataTable();
            DataTable condos_dt = new DataTable();
            condos_dt = clsobj.GetDreamHouse();
            dt = clsobj.GetMenuList();
            if (dt.Rows.Count > 0)
            {
                string PageName = dt.Rows[0]["PageName"].ToString();
                StrMenu.Append("<a class='toggleMenu' href='#'></a>");
                StrMenu.Append("<ul class='nav'>");
                StrMenu.Append("<li class='test' style='background:none;'><a href='../Home.aspx' title='Home' >Home</a></li>");
                StrMenu.Append("<li><a href=#>Exclusive Residential and Commercial </a>");//</li>
                StrMenu.Append("<ul >");
                StrMenu.Append("<li><a href='../my_haves_and_wants.aspx' title=''>My Haves and Wants</a></li>");
                StrMenu.Append("<li><a href='../Residential_Haves_and_Wants.aspx'>Residential Haves and Wants, Mostly Exclusive</a></li>");
                StrMenu.Append("<li><a href='../Ontario_Commercial_Mostly_Exclusive.aspx'>Ontario Commercial, Mostly Exclusive </a></li>");
                StrMenu.Append("<li><a href='../World_Commercial.aspx'> World Commercial</ a></li>");
                StrMenu.Append("<li><a href='../Business_Haves_and_Wants.aspx'>Business Haves and Wants </a></li>");
                StrMenu.Append("<li><a href='../For_Sale_by_Owner.aspx'>For Sale by Owner </a></li>");
                StrMenu.Append("</ul>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    clsobj.PageID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    dtSubmenu = clsobj.GetSubMenuBy_PageID();
                    if (dtSubmenu.Rows.Count > 0)
                    {
                        StrMenu.Append("<li><a href=#>" + dt.Rows[i]["PageName"] + "</a>");//</li>
                        StrMenu.Append("<ul>");
                        for (int j = 0; j < dtSubmenu.Rows.Count; j++)
                        {
                            StrMenu.Append("<li><a href='../StaticPages.aspx?PageID=" + dtSubmenu.Rows[j]["id"] + "' title='" + dtSubmenu.Rows[j]["PageName"] + "'>" + dtSubmenu.Rows[j]["PageName"] + "</a> </li>");
                        }
                        StrMenu.Append("</ul>");
                        StrMenu.Append("</li>");
                    }
                    else
                    {
                        StrMenu.Append("<li><a href='../StaticPages.aspx?PageID=" + dt.Rows[i]["id"] + "' title='" + dt.Rows[i]["PageName"] + "'>" + dt.Rows[i]["PageName"] + "</a>");//</li>
                    }
                }

                StrMenu.Append("<li><a href=#>Pre Constructions</a>");//</li>
                if (condos_dt.Rows.Count > 0)
                {
                    StrMenu.Append("<ul >");

                    for (int j = 0; j < condos_dt.Rows.Count; j++)
                    {
                        StrMenu.Append("<li><a href='../DreamHouseDetail.aspx?Id=" + condos_dt.Rows[j]["Id"] + "' title=''>" + condos_dt.Rows[j]["Title"] + "</a></li>");
                    }
                    StrMenu.Append("</ul>");
                }

                StrMenu.Append("<li>");
                StrMenu.Append("<a href='../Calculators.aspx' title='Calculators'>Calculators</a>");
                StrMenu.Append("</li>");
               
                StrMenu.Append("<li class='test' style='background:none;'><a href='home_worth.aspx?PageTitle=HomeWorth' title='Home Evaluation'>Home Evaluation</a></li>");
                StrMenu.Append("<li class='test' style='background:none;'><a href='ContactUs.aspx' title='Contact Us'>Contact Us</a></li>");
                StrMenu.Append("</ul>");
            }
            dynamicmenus.Text = StrMenu.ToString();
        }

        protected void SiteSetting()
        {
            try
            {
                DataTable dt = clsobj.GetSiteSettings();
                DataTable dt1 = clsobj.GetUserInfo();
                if (dt.Rows.Count > 0)
                {
                    //lblemailid.Text = Convert.ToString(dt.Rows[0]["Email"]);
                    siteTitle.Text = Convert.ToString(dt.Rows[0]["Title"]);
                   // lblemail.Text = Convert.ToString(dt.Rows[0]["Email"]);

                    //lblmobile.Text = Convert.ToString(dt.Rows[0]["PhoneNumber"]);
                    //lblfax.Text = Convert.ToString(dt.Rows[0]["Fax"]);
                   // lblemail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                   lblBrkrOneName.Text = Convert.ToString(dt1.Rows[0]["FirstName"]) + " " + Convert.ToString(dt1.Rows[0]["LastName"]);
                    //lbladdress.Text = Convert.ToString(dt1.Rows[0]["Address"]);
                    //lblBrkrTwoNme.Text = Convert.ToString(dt.Rows[0]["BrokerTwoName"]);
                    lblphn.Text = Convert.ToString(dt.Rows[0]["Mobile"]);
                    byte[] favimage = (byte[])dt.Rows[0]["Favicon.ico"];
                    if (favimage.Length > 0)
                    {
                        Session["MyFavicon"] = favimage;
                        favicon.Visible = true;
                        favicon.Href = "~/ShowFavicon.aspx";
                    }
                    else
                    {
                        favicon.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}