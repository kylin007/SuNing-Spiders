using Ivony.Html;
using Ivony.Html.Parser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skay.WebBot;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;
using System.ComponentModel;



namespace 苏宁数据抓取
{
    public partial class Form1 : Form
    {

        DataTable dt = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        public void button2_Click(object sender, EventArgs e)
        {
        }

        private void Form2()
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
        public delegate void del();
        bool flag1 = false;
        public void button3_Click(object sender, EventArgs e)
        {
            if(flag1)
            {
                MessageBox.Show("抓取已开始，请不要多次按键");
            }
            else
            {
                DataTable dta = new DataTable();
                SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=E_COMMERCE;Integrated Security=True");
                conn.Open();
                string sql = "select * from Item";
                SqlDataAdapter adap = new SqlDataAdapter(sql, conn);
                adap.Fill(dta);
                dt = dta;
                Thread th = new Thread(Find);
                th.Start();
                //Find();
            }
            
            
        }

        public void bindlabel(string val)
        {

        }


        public void Find()
        {
            #region  获取数据数量
             //DataTable dt7 = new DataTable();
             SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=E_COMMERCE;Integrated Security=True");
             conn.Open();
             //string sql = "select * from Date";//获取数据的个数
             //SqlDataAdapter adap = new SqlDataAdapter(sql, conn);
             //adap.Fill(dt7);
             flag1 = true;
             //int ii = dt7.Rows.Count;
            #endregion
             int ii = 117652;
             for (int i = 80; i < dt.Rows.Count; i++)//80
             {
                 //执行分类的用下面的
                 //if(i >= 82 && i<= 86)
                 //{
                 //}
                 //else if(i >= 458 && i<= 470)
                 //{
                 //}
                 //else
                 //{
                 //    if(i>86 && i<450)
                 //    {
                 //        i = 455;
                 //    }
                 //    else if(i>470)
                 //    {
                 //        MessageBox.Show("抓取完成");
                 //        break;
                 //    }
                 //    continue;
                 //}
                 DataTable dt1 = new DataTable();
                
                 object[] a = dt.Rows[i].ItemArray;
                 string url = (string)a[2];
                 int INDUSTRY_ID = 0;//-------------------------------------------------------一级分类编号
                 string INDUSTRY = null;
                 int PRODUCTTYPE_ID = 0;//----------------------------------------------------二级分类编号
                 string PRODUCTTYPE = null;
                 int PLATFORM_ID = 9;//-------------------------------------------------------购物平台的来源
                 string PLATFORM = "苏宁易购";

                 PRODUCTTYPE_ID = (int)a[3];

                 string sql = "select * from PRODUCTTYPE where ID = " + PRODUCTTYPE_ID;
                 SqlDataAdapter adap = new SqlDataAdapter(sql, conn);
                 adap.Fill(dt1);

                 object[] a1 = dt1.Rows[0].ItemArray;
                 INDUSTRY_ID = (int)a1[2];
                 INDUSTRY = a1[1].ToString();
                 PRODUCTTYPE = a[1].ToString();
                 this.label8.BeginInvoke(new del(() => { this.label8.Text = INDUSTRY; }));

                // label8.Text = INDUSTRY_ID.ToString();
                 
                // label9.Text = PRODUCTTYPE_ID.ToString();

                 this.label9.BeginInvoke(new del(() => { this.label9.Text = PRODUCTTYPE; }));


                 string list = "list";
                 string search = "search";
                 int flag = 0;
                 if (url.IndexOf(list) > 0)  //判断URL种类
                 {
                     flag = 0;
                 }
                 else if (url.IndexOf(search) > 0)
                 {
                     flag = 1;
                 }
                 else
                 {
                     continue;
                 }
                 HttpUtility http = new HttpUtility();
                 string html = null;
                 
                 try
                 {
                     html = http.GetHtmlText(url, "utf-8", "text/html; charset=utf-8");
                 }
                 catch
                 {
                     continue;
                 }
                 var documenthtml3 = new JumonyParser().Parse(html);

                 string booknum;//--页数
                 string book;
                 var boo = documenthtml3.FindFirst(".little-page span");
                 book = boo.FindFirst("i").InnerHtml();
                 book = book.Replace("\n", "");
                 booknum = boo.InnerHtml();
                 string regex = @"(\d+)\D+(\d+.)";
                 Match mstr = Regex.Match(booknum, regex);
                 booknum = mstr.Groups[2].Value;
                 //label10.Text = book;
                 //label11.Text = booknum;
                 this.label10.BeginInvoke(new del(() => { this.label10.Text = booknum; }));
                 this.label16.BeginInvoke(new del(() => { this.label16.Text = book; }));
                 int book1 = 1, book2 = 100;
                 book1 = int.Parse(book);
                 try
                 {
                     book2 = int.Parse(booknum);
                 }
                 catch
                 {
                     book2 = 100;
                 }
                 //book2 = int.Parse(booknum);

                 for(int x = 1 ; x<book2; x++)
                 {
                     this.label14.BeginInvoke(new del(() => { this.label14.Text = x.ToString(); }));
                     string uurl = url;
                     int book3 = x - 1;
                     if(flag == 0)
                     {
                         Match mstr2 = Regex.Match(url, regex);
                         string urlx = mstr2.Groups[2].Value;
                         uurl = "http://list.suning.com/0-" + urlx + "-" + book3 + ".html";
                     }
                     else
                     {
                         uurl = url + "&iy=0&cp="+book3;
                     }
                     try
                     {
                         html = http.GetHtmlText(uurl, "utf-8", "text/html; charset=utf-8");
                     }
                     catch
                     {
                         continue;
                     }
                     var documenthtml = new JumonyParser().Parse(html);
                     var items = documenthtml.Find(".clearfix li.product");
                     //int x4 = items.Count();

                     foreach (var item in items)
                     {

                         //richTextBox1.Text = "\n\n\n\n\n\n\n---------------获取成功第"+ii+"条----------------";
                         //ii++;
                         //ii = 78894;
                         //string TITLE = "无数据";//--------------title
                         string DETAIL_URL = "无数据";//--------------详细地址
                         try
                         {
                             var text = item.FindFirst(".sell-point a");
                             DETAIL_URL = text.Attribute("href").Value();
                             DETAIL_URL = "http:" + DETAIL_URL;
                         }
                         catch
                         {
                         }

                         int COMMEN_NUM = 0;//-----------总评
                         try
                         {
                             var text = item.FindFirst(".com-cnt");
                             var text1 = text.FindFirst("a");
                             COMMEN_NUM = int.Parse(text1.InnerHtml());
                         }
                         catch
                         {
                         }

                         string SHOPNAME = "无数据";//-------------店铺名称
                         try
                         {
                             var text = item.FindFirst(".seller");
                             SHOPNAME = text.Attribute("salesname").Value();
                         }
                         catch
                         {
                         }


                         int ta = 0, tb = 0;//-----编号
                         try
                         {
                             string tt = DETAIL_URL;
                             Match mstr1 = Regex.Match(tt, regex);
                             ta = int.Parse(mstr1.Groups[1].Value);
                             tb = int.Parse(mstr1.Groups[2].Value.Replace(".", ""));
                         }
                         catch
                         {
                         }

                         ////////////////////////////---------------------详情页信息-------------------------/////////////////////////////////

                         HttpUtility http1 = new HttpUtility();
                         string html1 = null;
                         try
                         {
                             html1 = http1.GetHtmlText(DETAIL_URL, "utf-8", "text/html; charset=utf-8");
                         }
                         catch
                         {
                             Thread.Sleep(3000);
                             http1 = new HttpUtility();
                             html1 = null;
                              try
                                {
                                    html1 = http1.GetHtmlText(DETAIL_URL, "utf-8", "text/html; charset=utf-8");
                                }
                              catch
                              {
                                  continue;
                              }
 
                         }
                         var documenthtml1 = new JumonyParser().Parse(html1);

                         string GOODSID = "无数据";  //--------商品编号
                         try
                         {
                             var text = documenthtml1.FindFirst(".imgzoom-memo");
                             GOODSID = text.FindLast("em").InnerHtml();
                         }
                         catch
                         {
                         }

                         string TITLE = "无数据";//-------------title
                         try
                         {
                             var text = documenthtml1.FindFirst("#itemDisplayName");
                             string text1 = text.InnerHtml();
                             TITLE = text1.Replace('\n', ' ').Replace('\t', ' ').Replace('\r', ' ').ToString();
                             TITLE = TITLE.Trim();
                         }
                         catch
                         {
                         }

                         int PROVINCE_ID = 0;//---------省份城市ID
                         int CITY_ID = 0;
                         try
                         {
                             DataTable dt2 = new DataTable();
                             DataTable dt3 = new DataTable();
                             //http://shop.suning.com/jsonp/70092198/shopinfo/shopinfo.html?callback=shopinfo
                             string url_shop = "http://shop.suning.com/jsonp/" + ta + "/shopinfo/shopinfo.html?callback=shopinfo";
                             string Area_Html = http.GetHtmlText(url_shop, "utf-8", "text/html;charset=utf-8", "");
                             var dom = (JObject)JsonConvert.DeserializeObject(Area_Html.Replace("shopinfo(", "").Replace(")", ""));
                             string PROVINCE = dom["companyProvince"].ToString();
                             string CITY = dom["companyCity"].ToString();

                             try
                             {
                                 
                                 CITY = "'" + CITY + "'";
                                 sql = "select CITY_CODE from CITY where CITY_NAME = " + CITY;
                                 adap = new SqlDataAdapter(sql, conn);
                                 adap.Fill(dt3);

                                 object[] a2 = dt3.Rows[0].ItemArray;
                                 CITY_ID = (int)a2[0];
                             }
                             catch
                             {
                             }
                             //省编号查询
                             try
                             {
                                 PROVINCE = "'" + PROVINCE + "'";
                                 sql = "select PROVINCE_CODE from PROVINCE where PROVINCE_NAME = " + PROVINCE;
                                 adap = new SqlDataAdapter(sql, conn);
                                 adap.Fill(dt2);

                                 object[] a3 = dt2.Rows[0].ItemArray;
                                 PROVINCE_ID = (int)a3[0];
                             }
                             catch
                             {
                             }
                         }
                         catch
                         {
                         }

                         //------------价格
                         string vendorType = "苏宁易购旗舰店";//店铺类型
                         //http://icps.suning.com/icps-web/getAllPriceFourPage/000000000155275268_0070092198_025_0250101_1_pc_showSaleStatus.vhtm?callback=showSaleStatus
                         decimal PRICE = 0;//价格
                         decimal DISCOUNT = 0;//促销价
                         try
                         {
                             string url_price = "http://icps.suning.com/icps-web/getAllPriceFourPage/000000000" + tb + "_00" + ta + "_025_0250101_1_pc_showSaleStatus.vhtm?callback=showSaleStatus";
                             string Area_Html = http.GetHtmlText(url_price, "utf-8", "text/html;charset=utf-8", "");
                             var dom = (JObject)JsonConvert.DeserializeObject(Area_Html.Replace("showSaleStatus(", "").Replace(");", ""));
                             string x2 = dom["saleInfo"][0]["netPrice"].ToString();
                             PRICE = Convert.ToDecimal(x2);

                             try
                             {
                                 vendorType = dom["saleInfo"][0]["vendorType"].ToString();
                             }
                             catch{  }
                             try
                             {
                                 string x3 = dom["saleInfo"][0]["promotionPrice"].ToString();
                                 DISCOUNT = Convert.ToDecimal(x3);
                             }
                             catch {
                                 DISCOUNT = PRICE;
                             }
                             
                         }
                         catch { }

                         int SALE_VOLUME = 0;//-------------销售总量
                         decimal SALE_AMOUNT = 0;
                         try
                         {
                             int A_index = html1.IndexOf("saleVolume");
                             int B_index = html1.IndexOf("imgHostNumber");
                             int B_A = B_index - A_index - 10;
                             string subString = html1.Substring(A_index, B_A);
                             string saleVolume = subString.Split(':')[1].ToString();
                             SALE_VOLUME = int.Parse(saleVolume);
                             SALE_AMOUNT = SALE_VOLUME * PRICE;
                         }
                         catch { }

                         //---------包装参数
                         string SHELVES_TIME = "无数据";
                         string pinpai = null;
                         int BRAND_ID = 177156;
                         try
                         {
                             var temm = documenthtml1.FindFirst("#itemParameter");
                             var tems = temm.Find("tr");
                             foreach (var tem in tems)
                             {
                                 try
                                 {
                                     string shop_name = tem.FindFirst(".name-inner span").InnerHtml();
                                     if (shop_name == "品牌")
                                     {
                                         pinpai = tem.FindFirst(".val").InnerText();
                                         try
                                         {
                                             DataTable dt4 = new DataTable();

                                             pinpai = "\'" + pinpai + "\'";
                                             sql = "select ID from BRAND where BRAND_NAME like " + pinpai;
                                             adap = new SqlDataAdapter(sql, conn);
                                             adap.Fill(dt4);

                                             object[] a4 = dt4.Rows[0].ItemArray;
                                             BRAND_ID = (int)a4[0];
                                         }
                                         catch
                                         {
                                         }
                                     }
                                     else if (shop_name == "上市时间")
                                     {
                                         SHELVES_TIME = tem.FindFirst(".val").InnerText();
                                     }

                                 }
                                 catch { }
                             }
                         }
                         catch { }


                         //--------好中差评

                         //http://review.suning.com/ajax/review_satisfy/style-000000000155275268-0070092198-----satisfy.htm?callback=satisfy
                         int POSITIVE_COMMEN = 0;
                         int MODERATE_COMMEN = 0;
                         int NEGATIVE_COMMEN = 0;
                         try
                         {
                             string url_COMMEN = "http://review.suning.com/ajax/review_satisfy/style-000000000" + tb + "-00" + ta + "-----satisfy.htm?callback=satisfy";
                             string Area_Html = null;
                             
                             try
                             {
                                 Area_Html = http.GetHtmlText(url_COMMEN, "utf-8", "text/html;charset=utf-8", "");
                             }
                             catch 
                             {
                                 Thread.Sleep(5000);
                                 Area_Html = http.GetHtmlText(url_COMMEN, "utf-8", "text/html;charset=utf-8", "");
                             }
                            var dom = (JObject)JsonConvert.DeserializeObject(Area_Html.Replace("satisfy(", "").Replace(")", ""));
                             string hao = dom["reviewCounts"][0]["fiveStarCount"].ToString();
                             POSITIVE_COMMEN = int.Parse(hao);
                             string zhong3 = dom["reviewCounts"][0]["threeStarCount"].ToString();
                             int zhong33 = int.Parse(zhong3);
                             string zhong4 = dom["reviewCounts"][0]["fourStarCount"].ToString();
                             int zhong44 = int.Parse(zhong4);
                             string cha1 = dom["reviewCounts"][0]["oneStarCount"].ToString();
                             int cha11 = int.Parse(cha1);
                             string cha2 = dom["reviewCounts"][0]["twoStarCount"].ToString();
                             int cha22 = int.Parse(cha2);
                             MODERATE_COMMEN = zhong33 + zhong44;
                             NEGATIVE_COMMEN = cha11 + cha22;
                         }
                         catch
                         { }
                         int COLLECTION_NUM = 0;//---------收藏人数
                         string REPUTATION = "苏宁无此无数据";//--------名誉
                         string SHOPTYPE = vendorType;//"苏宁易购旗舰店";//----店铺类型

                         DateTime sy = new DateTime();//sy为datetime型 
                         sy = System.DateTime.Today;//取当前日期给sy 
                         string year = sy.Year.ToString();//取年份 
                         string month = sy.Month.ToString();//取月份

                         int YEAR = int.Parse(year);
                         int MONTH = int.Parse(month);
                         this.label14.BeginInvoke(new del(() => { this.label14.Text = month; }));

                         this.label11.BeginInvoke(new del(() => { this.label11.Text = ii.ToString(); }));

                         string strstr = "商品编号 = " + GOODSID + "\n" + "一级分类的编号 = " + INDUSTRY_ID + "\n" + "二级分类的编号  = " + PRODUCTTYPE_ID + "\n" + "购物平台的来源 = " + PLATFORM_ID + "\n" + "品牌ID = " + BRAND_ID + "\n" + "省份 = " + PROVINCE_ID + "\n" + "店铺名称 = " + SHOPNAME + "\n" + "城市 = " + CITY_ID + "\n" + "title = " + TITLE + "\n" + "价格 = " + PRICE + "\n" + "促销价 = " + DISCOUNT + "\n" + "销售总量 = " + SALE_VOLUME + "\n" + "销售额 = " + SALE_AMOUNT + "\n" + "总评 = " + COMMEN_NUM + "\n" + "好评 = " + POSITIVE_COMMEN + "\n" + "中评 = " + MODERATE_COMMEN + "\n" + "差评  = " + NEGATIVE_COMMEN + "\n" + "收藏人数 = " + COLLECTION_NUM + "\n" + "名誉 = " + REPUTATION + "\n" + "店铺类型 = " + SHOPTYPE + "\n" + "上市时间 = " + SHELVES_TIME + "\n" + "年 = " + YEAR + "\n" + "月 = " + MONTH + "\n" + "详细地址 = " + DETAIL_URL;

                         //this.richTextBox1.BeginInvoke(new del(() => { this.richTextBox1.Text = "----------------插入成功-------------" + "\n" + strstr; }));
                         this.richTextBox1.BeginInvoke(new del(() => { this.richTextBox1.Text = strstr; }));
                         ii++;

                         #region  去重 插入
                         //DataTable dt6 = new DataTable();

                         ////string GOODSID1 = "'" + GOODSID + "'";
                         //string DETAIL_URL1 = "'" + DETAIL_URL + "'";
                         //sql = "select * from Date where DETAIL_URL = " + DETAIL_URL1 + "and MONTH = " + MONTH + "and YEAR = " + YEAR;
                         //adap = new SqlDataAdapter(sql, conn);
                         //adap.Fill(dt6);

                         //Thread.Sleep(1000);
                         //if (dt6.Rows.Count != 0)
                         //{
                         //    this.richTextBox1.BeginInvoke(new del(() => { this.richTextBox1.Text = strstr; }));
                         //}
                         //else
                         //{
                         //    SHOPNAME = SHOPNAME.Replace("'", "-").Replace("’", "-");
                         //    TITLE = TITLE.Replace("’", "-").Replace("'", "-");
                         //    PLATFORM = "苏宁易购";
                         //    //--------------------------------
                         //    //
                         //    //
                         //    //
                         //    //
                         //    //------------------------------
                         //    //sql = string.Format("INSERT INTO Date VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')", GOODSID, INDUSTRY, PRODUCTTYPE, PLATFORM, BRAND_ID, PROVINCE_ID, SHOPNAME, CITY_ID, TITLE, PRICE, DISCOUNT, SALE_VOLUME, SALE_AMOUNT, COMMEN_NUM, POSITIVE_COMMEN, MODERATE_COMMEN, NEGATIVE_COMMEN, COLLECTION_NUM, REPUTATION, SHOPTYPE, SHELVES_TIME, YEAR, MONTH, DETAIL_URL);
                         //    ////sql7.Replace("'", "-");
                         //    //SqlCommand com = new SqlCommand(sql, conn);
                         //    //com.ExecuteNonQuery();
                         //    this.richTextBox1.BeginInvoke(new del(() => { this.richTextBox1.Text = "----------------插入成功-------------"+"\n" + strstr; }));
                         //    ii++;
                         //}
                         #endregion

                     }//tems列表循环

                 }//book循环
                
             }//dt内URL循环
             conn.Close();

             MessageBox.Show("苏宁易购抓取完成");
        }//find函数循环

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
