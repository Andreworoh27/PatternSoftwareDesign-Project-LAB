﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApp_LAB.ado_model;
using WebApp_LAB.controller;

namespace WebApp_LAB.view.album
{
    public partial class UpdateAlbum : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int albumId = Convert.ToInt32(Request.QueryString["albumId"]);
                int artistId = Convert.ToInt32(Request.QueryString["artistId"]);
                Album a = AlbumController.GetAlbumByID(albumId);
                nameTxt.Text = a.AlbumName;
                descTxt.Text = a.AlbumDescription;
                priceTxt.Text = Convert.ToInt32(a.AlbumPrice).ToString();
                stockTxt.Text = Convert.ToInt32(a.AlbumStock).ToString();
            }
        }

        protected void updateBtn_Click(object sender, EventArgs e)
        {
            int artistId = Convert.ToInt32(Request.QueryString["artistId"]);
            int albumId = Convert.ToInt32(Request.QueryString["albumId"]);
            Album a = AlbumController.GetAlbumByID(albumId);

            string name = nameTxt.Text;
            string desc = descTxt.Text;
            int price = 0;
            int stock = 0;

            try
            {
                price = Convert.ToInt32(priceTxt.Text);
            }
            catch
            {
                price = 0;
            }
            try
            {
                stock = Convert.ToInt32(stockTxt.Text);
            }
            catch
            {
                stock = 0;
            }
            string fileName = "";

            nameError.Text = AlbumController.checkAlbumName(name);
            descError.Text = AlbumController.checkAlbumDescription(desc);
            priceError.Text = AlbumController.checkAlbumPrice(price);
            stockError.Text = AlbumController.checkAlbumStock(stock);

            if (albumImg.HasFile)
            {
                fileName = albumImg.PostedFile.FileName;
                string extension = Path.GetExtension(fileName);
                int fileSize = albumImg.PostedFile.ContentLength;
                imageError.Text = AlbumController.checkAlbumImageName(extension, fileSize);
                albumImg.SaveAs(Server.MapPath("~/assets/albums/") + fileName);
            }
            else
            {
                fileName = a.AlbumImage;
            }

            if (nameError.Text.Equals("") && descError.Text.Equals("") && priceError.Text.Equals("") && stockError.Text.Equals("") && imageError.Text.Equals(""))
            {
                if (albumImg.HasFile)
                {
                    string path = Server.MapPath("~/assets/albums/" + AlbumController.GetAlbumByID(albumId).AlbumImage);
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    albumImg.SaveAs(Server.MapPath("~/assets/albums/") + fileName);
                }

                AlbumController.UpdateAlbumByID(albumId, name, artistId, desc, price, stock, fileName);
                Response.Redirect("~/view/artist/ArtistDetail.aspx?artistId=" + artistId);
            }

        }
    }
}