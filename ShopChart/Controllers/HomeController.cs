using iTextSharp.text;
using iTextSharp.text.pdf;
using ShopChart.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace ShopChart.Controllers
{
    public class HomeController : Controller
    {
        public string selectedPID = "";
        ShopCartDbEntities18 db = new ShopCartDbEntities18();
        public ActionResult Index()
        {
            //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Cart]");
            //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [PurchasedData]");
            //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [BillingData]");
            //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [RegisterCustomer]");
            //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Contact]");
           
            int count = db.Carts.Count();
            ViewBag.CountOfCart = count;
            return View();
        }
          public FileResult CreatePdf()  
        {  
            MemoryStream workStream =   new MemoryStream();  
            StringBuilder status    =    new StringBuilder("");

            DateTime dTime = DateTime.Now;  
            //file name to be created   
            string strPDFFileName = string.Format("Billing_Receipt " + dTime.ToString("yyyy MM dd") + "-" + ".pdf");  
            Document doc = new Document();  
            doc.SetMargins(0f,0f,0f,0f);  
            //Create PDF Table with 8 columns  
            PdfPTable tableLayout = new PdfPTable(8);  
            doc.SetMargins(0f, 0f, 0f, 0f);  
            //Create PDF Table  
  
            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);  
  
  
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;  
            doc.Open();  
  
            //Add Content to PDF  
            doc.Add(Add_Content_To_PDF(tableLayout));  
  
            // Closing the document  
            doc.Close();  
  
            byte[] byteInfo = workStream.ToArray();  
            workStream.Write(byteInfo, 0, byteInfo.Length);  
            workStream.Position = 0;  
  
  
            return File(workStream, "application/pdf", strPDFFileName);  
  
        }  
  
    protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)  
    {  
  
            float[] headers = {50,50,50,50,50,80,80,100}; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;  
            //Add Title to the PDF file at the top  
             string tempemail = Session["tempEmail"].ToString();
            List<PurchasedData> employees = db.PurchasedDatas.Where(t => t.Email == tempemail).ToList();

           // double totalprice = 0;
            //totalprice = Convert.ToDouble(db.PurchasedDatas.Sum(i => i.Price));

            tableLayout.AddCell(new PdfPCell(new Phrase(Session["UserFirstName"] + " your Purchased Items Receipt ", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {  
                Colspan = 12, Border = 2, PaddingBottom = 20,PaddingTop=20,
                HorizontalAlignment = Element.ALIGN_CENTER  
            });  
  
  
            ////Add header  
            AddCellToHeader(tableLayout, "ProductID");  
            AddCellToHeader(tableLayout, "Price");  
            AddCellToHeader(tableLayout, "Category");  
            AddCellToHeader(tableLayout, "Color");  
            AddCellToHeader(tableLayout, "Brand");  
            AddCellToHeader(tableLayout, "Purchased Date");    
            AddCellToHeader(tableLayout, "Product");   
            AddCellToHeader(tableLayout, "Customer Email");
            //AddCellToHeader(tableLayout,"Total Price");
            ////Add body  
  
            foreach(var emp in employees)   
            {  
                AddCellToBody(tableLayout, emp.ProductID);  
                AddCellToBody(tableLayout, emp.Price.ToString());  
                AddCellToBody(tableLayout, emp.Category);  
                AddCellToBody(tableLayout, emp.Color);  
                AddCellToBody(tableLayout, emp.Brand); 
                AddCellToBody(tableLayout, emp.Entery_Date);
                AddCellToBody(tableLayout,emp.ChooseImage);
                
                //"~/Images/@item.ChooseImage/emp.ChooseImage"); 
               // <img src="~/Images/@item.ChooseImage" width="30" height="20" />
                AddCellToBody(tableLayout, emp.Email); 
  
            }
            //AddCellToBody(tableLayout, totalprice.ToString());
            return tableLayout;  
        }  
        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)  
        {  
  
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))  
            {  
                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)  
            });  
        }  
  
        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)  
        {  
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))  
             {  
                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)  
            });  
        }  



        public ActionResult MyAccount()
        {
            ViewBag.ID = "500100";
            string tempemail = "";
            if (Session["user"] != null)
            {
                tempemail = Session["tempEmail"].ToString();
            }
            List<Object> myModel = new List<object>();
            myModel.Add(db.Carts.ToList());
            myModel.Add(db.PurchasedDatas.Where(t => t.Email == tempemail).ToList());
            //other way
            //var query = from a in db.PurchasedDatas
            //            where a.Email.Contains(tempemail)
            //            select a;
            //Session["PurchasedData"] = query;

            return View(myModel);
        }

        public ActionResult Logout()
        {
            Session["user"] = null;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CheckOut()
        {
            return View(db.PurchasedDatas.ToList());
        }
        public ActionResult RemoveCart(int id)
        {

            Cart c = db.Carts.Find(id);
            db.Carts.Remove(c);
            db.SaveChanges();
            return RedirectToAction("ShopCart");
        }

        public ActionResult ShopCart(string id)
        {
            Response.Write(id);
            //string userEmail = Session["tempEmail"].ToString();
            ProductInfo p = db.ProductInfoes.Find(id);
            if (p != null)
            {
                ViewBag.ID = p.ProductID;
                ViewBag.PRICE = p.Price;
                ViewBag.COLOR = p.Color;
                ViewBag.CATEGORY = p.Category;
                ViewBag.Brand = p.Brand;
                ViewBag.DATEENTERY = p.Entery_Date;
                ViewBag.ChooseImage = p.ChooseImage;

                Cart c = new Cart
                {
                    ID = 1,
                    ProductID = id,
                    Price = p.Price,
                    Color = p.Color,
                    Category = p.Category,
                    Brand = p.Brand,
                    Entery_Date = DateTime.Now.ToShortDateString(),
                    ChooseImage = p.ChooseImage,                
                };
                db.Carts.Add(c);
                db.SaveChanges();

                Session["Cart"] = db.Carts.ToList();           
                return View(db.Carts.ToList());
            }
            else
            {
                return View(db.Carts.ToList());
            }
        }
        public ActionResult SearchProduct()
        {
            string searchedId = Request["SearchedID"];
            ProductInfo p = db.ProductInfoes.Find(searchedId);
            if (p == null)
            {
                ViewBag.Msg = "Sorry, we couldn't understand this search. Please try saying this another way.";
                return View();
            }
            ViewBag.ID = p.ProductID;
            ViewBag.PRICE = p.Price;
            ViewBag.COLOR = p.Color;
            ViewBag.CATEGORY = p.Category;
            ViewBag.Brand = p.Brand;
            ViewBag.DATEENTERY = p.Entery_Date;
            ViewBag.ChooseImage = p.ChooseImage;

            return View();
        }

        public ActionResult SendPassword()
        {
            //Response.Write(r.Email);
            string email = Request["Email"];
            RegisterCustomer r = db.RegisterCustomers.Find(email);

            if (r == null)
            {
                return View("NotFound");
            }
            ViewBag.Email = r.Email;
            ViewBag.Password = r.Password;
            ViewBag.FirstName = r.FirstName;
            ViewBag.LastName = r.LastName;
            ViewBag.Title = r.Title;
            ViewBag.DOB = r.DOB;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("farahramzan39@gmail.com", "Farah's Shopping Cart");
            mail.To.Add("farahramzan39@gmail.com");
            mail.IsBodyHtml = true;
            mail.Subject = "Get Password";
            mail.Body = r.FirstName + " Your Password is " + r.Password;
            mail.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential("farahramzan39@gmail.com", "get-lost");
            smtp.EnableSsl = true;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.Send(mail);
            return View();
        }

       
        [HttpPost]
        public ActionResult Contact(Contact c)
        {
            if (ModelState.IsValid)
                {
                    db.Contacts.Add(c);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            return View(c);
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BillingDetails(BillingData bd)
        {
            if (ModelState.IsValid)
            {
                db.BillingDatas.Add(bd);
                db.SaveChanges();
                return RedirectToAction("PurchasedMessage");
            }
            return View(bd);
        }
        public ActionResult BillingDetails()
        {
            return View();
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        public ActionResult PurchasedMessage()
        {

            int count = db.Carts.Count();
            var temps = db.Carts.ToList();
            string email = "";
            if (Session["user"] != null)
            {
                email = Session["tempEmail"].ToString();
                foreach (var temp in temps)
                {
                    db.PurchasedDatas.Add(new PurchasedData()
                    {
                        ID = 1,
                        ProductID = temp.ProductID,
                        Price = temp.Price,
                        Category = temp.Category,
                        Color = temp.Color,
                        Brand = temp.Brand,
                        Entery_Date = DateTime.Now.ToShortDateString(),
                        ChooseImage = temp.ChooseImage,
                        Email = email
                    });
                }
                int flag = db.SaveChanges();

                if (flag > 0)
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Cart]");
                    // var records = db.Carts.Where(a => a.Email== email).ToList();
                    //foreach (var  item in records)
                    //{ 
                    //    db.Carts.Remove(item);
                    //}
                    //db.SaveChanges();

                    string tempemail = Session["tempEmail"].ToString();
                    List<PurchasedData> outputList = db.PurchasedDatas.Where(t => t.Email == tempemail).ToList();
                    List<PurchasedData> inputList = new List<PurchasedData>();

                    foreach (var item in outputList)
                    {
                        PurchasedData outputItem = new PurchasedData(item);
                        inputList.Add(outputItem);
                    }
                    //string file = @"D:\DownloadFile\Receipt.txt";
                  
                    //TextWriter tw = new StreamWriter(file);
                    //foreach (var item in inputList)
                    //{
                    //    tw.WriteLine(item.ToString());
                    //}
                    //tw.Close();

                    
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("farahramzan39@gmail.com", "Farah's Shopping Cart");
                    mail.To.Add(tempemail);
                    mail.IsBodyHtml = true;
                    mail.Subject = "Confirm Purchased Message";
                    mail.Body = Session["UserFirstName"].ToString() + " Thank You for Purchasing.";

                    System.Net.Mail.Attachment attachment;
                    MemoryStream workStream = new MemoryStream();
                    attachment = new System.Net.Mail.Attachment(workStream, "txt");
                    attachment.ContentDisposition.FileName = "Receipt " + DateTime.Now.ToString("yyyy MM dd") + ".txt";
                    mail.Attachments.Add(attachment);

                    mail.Priority = MailPriority.High;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    //smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new System.Net.NetworkCredential("farahramzan39@gmail.com", "get-lost");
                    smtp.EnableSsl = true;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.Send(mail);
                    return View();
                }
                return View("Error");

            }
            return View("Error");

            //return View();
        }


        
        public ActionResult Download()
        {
            string file = @"D:\DownloadFile\foo.txt";
            string contentType = "application/txt";
            return File(file, contentType, Path.GetFileName(file));
        }
        public ActionResult About_Us()
        {
            int count = db.Carts.Count();
            ViewBag.CountOfCarts = count;
            return View();
        }
       
        public ActionResult Compair()
        {
             int count = db.Carts.Count();
            ViewBag.CountOfCarts = count;
            return View();
        }
        public ActionResult Footer()
        {
            return View();
        }

        public ActionResult Four_Col()
        {
             int count = db.Carts.Count();
            ViewBag.CountOfCarts = count;
            return View();
        }
        public ActionResult General()
        {
             int count = db.Carts.Count();
            ViewBag.CountOfCarts = count;
            return View(db.ProductInfoes.ToList());
        }

        public ActionResult Grid_View()
        {
             int count = db.Carts.Count();
            ViewBag.CountOfCarts = count;
            return View();
        }
        public ActionResult Header()
        {
           
            return View();
        }
        public ActionResult List_View()
        {
            int count = db.Carts.Count();
            ViewBag.CountOfCarts = count;
            return View();
        }

        public ActionResult Product_Details(string id)
        {
            Response.Write(id);
            TempData["TempID"] = id;
            ProductInfo info = db.ProductInfoes.Find(id);
            if (info == null)
            {
                return View("Error");
            }
            return View(info);
        }
        public ActionResult Products()
        {
            return View();
        }
        public ActionResult SuccessRegistration()
        {
            return View();
        }
         public JsonResult AlreadyExists(string data)
        {
            System.Threading.Thread.Sleep(200);
            string email = Request["email"];
            var searchdata = db.RegisterCustomers.Where(x => x.Email == data).SingleOrDefault();
            if (searchdata != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }
        
        [HttpPost]
        public ActionResult Register(RegisterCustomer c)
        {
            try
            {

                db.RegisterCustomers.Add(c);
                db.SaveChanges();
                return RedirectToAction("SuccessRegistration");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "CustomerInfo", "Create"));
            }
        }
        public ActionResult Register()
        {
            return View();
        }
       
        public JsonResult IsCredentialsCorrect(string email, string password)
        {
            bool res = false;
            RegisterCustomer u = db.RegisterCustomers.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (u != null)
            {
                res = true;
            }

            return Json(new { status = res }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult DeleteAccount(string email)
        {
            Response.Write(email);

            RegisterCustomer u = db.RegisterCustomers.Find(email);
            if (u != null)
            {
                db.RegisterCustomers.Remove(u);
                db.SaveChanges();
                //Session["user"] = null;
                return View("Index");
            }
            return View("Error");
        }
        public JsonResult FindUserAccount()
        {
            string email = Request["email"];
            bool result = false;
            RegisterCustomer u = db.RegisterCustomers.Find( email);
            if (u != null)
            {
                result = true;
            }
            if (result)
                Session["user"] = null;
            
            return Json(new { status = result },JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login(string buyID)
        {

            return View();
        }

        public ActionResult SignInData(String buyID)
        {
            Response.Write(buyID);
            string email = Request["Email"];
            Session["tempEmail"] = email;
            string password = Request["Password"];
            RegisterCustomer s = db.RegisterCustomers.Find(email);
            if (s == null)
            {
                return View("NotFound");
            }
            if (!(s.Password.Equals(password)))
            {
                return View("NotFound");
            }
            Session["user"] = s;
            Session["UserFirstName"] = s.FirstName;


            if (Session["taketobuy"] == "true")
            {
                Session["taketobuy"] = "false";
                //return RedirectToAction("Confirm");//change
                return RedirectToAction("ShopCart");
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult buy()
        {          
            int count = db.Carts.Count();
            var temps = db.Carts.ToList();
            string email="";
            if(Session["user"] != null)
            {
                email = Session["tempEmail"].ToString();
                if (temps != null && Session["user"] != null)
                {
                    //foreach (var temp in temps)
                    //{
                    //    db.PurchasedDatas.Add(new PurchasedData()
                    //    {
                    //        ID = 1,
                    //        ProductID = temp.ProductID,
                    //        Price = temp.Price,
                    //        Category = temp.Category,
                    //        Color = temp.Color,
                    //        Brand = temp.Brand,
                    //        Entery_Date = DateTime.Now.ToShortDateString(),
                    //        ChooseImage = temp.ChooseImage,
                    //        Email = email
                    //    });
                    //}
                    //int flag = db.SaveChanges();

                    //if (flag > 0)
                    //{
                    //    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Cart]");
                    //    // var records = db.Carts.Where(a => a.Email== email).ToList();
                    //    //foreach (var  item in records)
                    //    //{ 
                    //    //    db.Carts.Remove(item);
                    //    //}
                    //    //db.SaveChanges();
                    //    ViewBag.Confirm = "Yes";
                    //    return RedirectToAction("BillingDetails", "Home");//change
                    //}
                    return RedirectToAction("BillingDetails", "Home");//change
                }

                return RedirectToAction("BillingDetails");//change

                }
            else
            {
                Session["taketobuy"] = "true";
                return RedirectToAction("Login");
            }
           // return View("Index");
        }
       
        public ActionResult Confirm()
        {
            return View(db.Carts.ToList());
        }
        [HttpPost]
        public ActionResult UpdateProfile(FormCollection fc)
        {
            string email = fc["email"];
            RegisterCustomer rc = db.RegisterCustomers.Find(email);
            if (rc != null)
            {
                rc.Email     = email;
                rc.FirstName = fc["fname"];
                rc.LastName  = fc["lname"];
                rc.Title     = fc["title"];
                rc.Password  = fc["password"];
                rc.DOB       = fc["dob"];

                db.Entry(rc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyAccount");
            }
            else
            {
                return View("Error");
            }
        }
        public ActionResult Cart()
        {
            return View(db.ProductInfoes.ToList());
        }
       
        public ActionResult AdminLogin()
        {
            string email = Request["Email"];
            string pass = Request["Password"];

            if (Session["Email"] != null && Session["Password"] != null && Session["Email"].ToString().Equals(email)
                && Session["Password"].ToString().Equals(pass))
            {
                return View("../Home/Cart");
            }
            else
            {
                RegisterAdmin s = db.RegisterAdmins.Find(email);
                if (s == null)
                {
                    return View("../Home/NotFound");
                }
                else
                {
                    if (s != null && s.Email.Equals(email) && s.Password.Equals(pass) && s.Status.Equals("admin"))
                    {
                        Session["Email"] = s.Email;
                        Session["Password"] = s.Password;
                        Session["Status"] = s.Status;
                        return RedirectToAction("Index", "Home");

                    }
                }

                return View("../Home/Error");
            }
        }
        public ActionResult AdLogin()
        {
            return View();
        }
        public ActionResult AdminLogout()
        {
            Session["Email"] = null;
            Session["Password"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Update(string id)
        {
            if (Session["Email"] != null)
            {
                if (Session["Email"].ToString().Equals("farahramzan39@gmail.com") && Session["Password"].ToString().Equals("123")
                && Session["Status"].ToString().Equals("admin"))
                {
                    ProductInfo p = db.ProductInfoes.Find(id);
                    if (p == null)
                    {
                        return View("../Home/NotFound");
                    }
                    return View(p);
                }
                return View("AdLogin");
               }
            else
            {
                if (Session["Email"] == null)
                {
                    return View("AdLogin");
                   }
                return View("../Home/Error");
            }
        }
        [HttpPost]
        public ActionResult Update(ProductInfo p)
        {
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Cart", "Home");
        }
        public ActionResult Add()
        {
            if (Session["Email"] != null)
            {
                if (Session["Email"].ToString().Equals("farahramzan39@gmail.com") && Session["Password"].ToString().Equals("123")
                && Session["Status"].ToString().Equals("admin"))
                {
                    return View();
                }
                return View("../Home/AdLogin");
            }
            else
            {
                if (Session["Email"] == null)
                {
                    return View("../Home/AdLogin");
                }
                return View("../Home/Error");
            }
        }

        [HttpPost]
        public ActionResult Add(ProductInfo p, HttpPostedFileBase file)
        {

            if (file != null)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/Images/" + ImageName);

                // save image in folder
                file.SaveAs(physicalPath);

                //save new record in database
                ProductInfo newRecord = new ProductInfo();
                p.ProductID = Request.Form["ProductId"];
                p.ChooseImage = ImageName;
                p.Price = Convert.ToInt32(Request.Form["Price"]);
                p.Color = Request.Form["Color"];
                p.Category = Request.Form["Category"];
                p.Brand = Request.Form["Brand"];
                p.Entery_Date = Request.Form["Entery_Date"];


                db.ProductInfoes.Add(p);
                db.SaveChanges();
                return RedirectToAction("Cart", "Home");
            }

            return View("../Home/Error");
        }
        public ActionResult Remove(string id = "")
        {
            if (Session["Email"] != null)
            {
                if (Session["Email"].ToString().Equals("farahramzan39@gmail.com") && Session["Password"].ToString().Equals("123")
                && Session["Status"].ToString().Equals("admin"))
                {
                    ProductInfo p = db.ProductInfoes.Find(id);
                    if (p == null)
                    {
                        return View("../Home/NotFound");
                    }
                    return View(p);
                }
                return View("../Home/AdLogin");
                //return Content("<script language='javascript' type='text/javascript'>alert('You must Login as Admin to manage Stock!');</script>");
            }
            else
            {
                if (Session["Email"] == null)
                {
                    return View("../Home/AdLogin");
                    // return Content("<script language='javascript' type='text/javascript'>alert('You must Login as Admin to manage Stock!');</script>");
                }
                return View("../Home/Error");
            }
        }
        [HttpPost, ActionName("Remove")]
        public ActionResult RemoveConfirmed(string id)
        {

            ProductInfo p = db.ProductInfoes.Find(id);
            db.ProductInfoes.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Cart", "Home");
        }

        public ActionResult Sidebar()
        {
            return View();
        }
        public ActionResult Three_Col()
        {
            return View();
        }

        public ActionResult Forget_Password()
        {
            return View();
        }
        public void setSelectedID(string ID)
        {

            selectedPID = ID;
        }
        public string getSelectedID()
        {
            return selectedPID;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
