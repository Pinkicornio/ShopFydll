using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace ShopfyDBLibrary
{
    public class DB
    {
       
        private string dbName = "sql4415325";
        private static MySqlConnection conn = null;
        private static MySqlCommand cmd;
        private static MySqlTransaction trans;

        public bool startConnection()
        {
         string server = "sql4.freesqldatabase.com";
         string user = "sql4415325";
         string password = "kG5qWmg8qi";
         string port = "3306";
         string cs;

        cs = "server=" + server + ";user=" + user + ";port=" + port + ";password=" + password + ";";

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool stopConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX----USUARIOS----XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        public bool userInsert(string username, string pwd, string email, string rol, int cloud)
        {
            DateTime _date = DateTime.Now;

            string sql = "INSERT INTO " + dbName + ".USERS (`USERNAME`, `PWD`, `EMAIL`, `CREATION_DATE`, `ROL`, `CLOUD`) VALUES (@username, @pwd, @email, @creation_date, @rol, @cloud)";

            try
            {
                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pwd", pwd);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@creation_date", _date);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.Parameters.AddWithValue("@cloud", cloud);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Commit();
                return false;
            }
        }

        public bool userUpdate(int user_id, string username, string pwd, string email, string rol, int cloud)
        {
            string sql = "UPDATE " + dbName + ".USERS SET USERNAME=@username, PWD=@pwd, EMAIL=@email, ROL=@rol, CLOUD=@cloud WHERE USER_ID = @user_id";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@user_id", user_id);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pwd", pwd);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.Parameters.AddWithValue("@cloud", cloud);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool userDelete(int user_id)
        {
                string sql = "DELETE FROM " + dbName + ".`USERS` WHERE `USERS`.`USER_ID` = @user_id";
            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@user_id", user_id);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

      

        public DataTable userSelection(string criteria, string criteriaData1, string criteriaData2)
        {
            string sql = "";
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            DataTable selectTable = new DataTable();

            switch (criteria.ToUpper())
            {
                case "USERNAME"://SELECT BY USERS
                    sql = "SELECT * FROM " + dbName + ".USERS WHERE USERNAME=@username";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "EMAIL"://SELECT BY EMAIL
                    sql = "SELECT * FROM " + dbName + ".USERS WHERE EMAIL=@email";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@email", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "ROL"://SELECT BY ROL
                    sql = "SELECT * FROM " + dbName + ".USERS WHERE ROL=@rol";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@rol", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "USER_ID"://SELECT BY ID
                    sql = "SELECT * FROM " + dbName + ".USERS WHERE USER_ID = @user_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "CREATION_DATE"://SELECT BY DATE FORMAT:dd/mm/yyyy dd/mm/yyyy
                    sql = "SELECT * FROM " + dbName + ".USERS WHERE CREATION_DATE BETWEEN @date1 AND @date2";
                    cmd = new MySqlCommand(sql, conn);

                    DateTime date1 = DateTime.ParseExact(criteriaData1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime date2 = DateTime.ParseExact(criteriaData2, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    cmd.Parameters.AddWithValue("@date1", date1.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@date2", date2.ToString("yyyy-MM-dd"));
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                default://GENERAL SELECT
                    sql = "SELECT * FROM " + dbName + ".USERS";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;
            }

            return selectTable;
        }


        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX----CATEGORIAS----XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        public bool categoryInsert(string category_name)
        {
            string sql = "INSERT INTO " + dbName + ".CATEGORIES (`NAME`) VALUES (@name)";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;

                cmd.Parameters.AddWithValue("@name", category_name);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool categoryDelete(int category_id)
        {
            string sql = "DELETE FROM " + dbName + "`.CATEGORIES` WHERE `CATEGORIES`.`CATEGORY_ID` = @category_id";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@category_id", category_id);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool categoryUpdate(int category_id, string category_name)
        {
            string sql = "UPDATE " + dbName + ".CATEGORIES SET NAME=@category_name WHERE CATEGORY_ID = @category_id";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;

                cmd.Parameters.AddWithValue("@category_id", category_id);
                cmd.Parameters.AddWithValue("@category_name", category_name);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }



        public DataTable categorySelection(string criteria, string criteriaData)
        {
            string sql = "";
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            DataTable selectTable = new DataTable();

            switch (criteria.ToUpper())
            {
                case "NAME"://SELECT BY CATEGORY_NAME
                    sql = "SELECT * FROM " + dbName + ".CATEGORIES WHERE NAME=@category_name";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@category_name", criteriaData);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "CATEGORY_ID"://SELECT BY CATEGORY_ID
                    sql = "SELECT * FROM " + dbName + ".CATEGORIES WHERE CATEGORY_ID=@category_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@category_id", criteriaData);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                default://GENERAL SELECT
                    sql = "SELECT * FROM " + dbName + ".CATEGORIES";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;
            }

            return selectTable;
        }


        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX----SUBCATEGORIAS----XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        public bool subcategoryInsert(string subcategory_name)
        {
            string sql = "INSERT INTO " + dbName + ".SUBCATEGORIES (`NAME`) VALUES (@name)";


            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@name", subcategory_name);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool subcategoryDelete(int subcategory_id)
        {
            string sql = "DELETE FROM " + dbName + ".`SUBCATEGORIES` WHERE `SUBCATEGORIES`.`SUBCATEGORY_ID` = @subcategory_id";

            try
            {
                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;

                cmd.Parameters.AddWithValue("@subcategory_id", subcategory_id);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool subcategoryUpdate(int subcategory_id, string subcategory_name)
        {
            string sql = "UPDATE " + dbName + ".`SUBCATEGORIES` SET `NAME`=@subcategory_name WHERE `SUBCATEGORIES`.`SUBCATEGORY_ID` = @subcategory_id";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@subcategory_id", subcategory_id);
                cmd.Parameters.AddWithValue("@subcategory_name", subcategory_name);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }


        public DataTable subcategorySelection(string criteria, string criteriaData)
        {
            string sql = "";
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            DataTable selectTable = new DataTable();

            switch (criteria.ToUpper())
            {
                case "NAME"://SELECT BY CATEGORY_NAME
                    sql = "SELECT * FROM " + dbName + ".SUBCATEGORIES WHERE NAME=@subcategory_name";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@subcategory_name", criteriaData);
                    rdr = cmd.ExecuteReader();  
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "SUBCATEGORY_ID"://SELECT BY CATEGORY_ID
                    sql = "SELECT * FROM " + dbName + ".SUBCATEGORIES WHERE SUBCATEGORY_ID=@subcategory_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@subcategory_id", criteriaData);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                default://GENERAL SELECT
                    sql = "SELECT * FROM " + dbName + ".SUBCATEGORIES";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;
            }

            return selectTable;
        }


        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX----PRODUCTOS----XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        public bool productInsert(string name, string brand, float price, int stock, int category_id, int subcategory_id)
        {
            string sql = "INSERT INTO " + dbName + ".PRODUCTS (`NAME`, `BRAND`, `PRICE`, `STOCK`, `CATEGORY_ID`, `SUBCATEGORY_ID`) VALUES (@name, @brand, @price, @stock, @category_id, @subcategory_id)";
            try
            {
                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@brand", brand);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@category_id", category_id);
                cmd.Parameters.AddWithValue("@subcategory_id", subcategory_id);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return false;
            }
        }

        public bool productDelete(int product_id)
        {
            string sql = "DELETE FROM " + dbName + ".`PRODUCTS` WHERE `PRODUCTS`.`PRODUCT_ID` = @product_id";
            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@product_id", product_id);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return false;
            }
        }

        public bool productUpdate(int product_id, string name, string brand, float price, int stock, int category_id, int subcategory_id)
        {
            string sql = "UPDATE " + dbName + ".PRODUCTS SET NAME=@name, BRAND=@brand, PRICE=@price, STOCK=@stock, CATEGORY_ID=@category_id, SUBCATEGORY_ID=@subcategory_id WHERE PRODUCT_ID = @product_id";
            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@brand", brand);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@category_id", category_id);
                cmd.Parameters.AddWithValue("@subcategory_id", subcategory_id);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return false;
            }
        }

        public DataTable productSelection(string criteria, string criteriaData1, string criteriaData2)
        {
            string sql = "";
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            DataTable selectTable = new DataTable();

            switch (criteria.ToUpper())
            {
                case "CATEGORY_ID"://SELECT BY CATEGORY_ID
                    sql = "SELECT * FROM " + dbName + ".PRODUCTS WHERE CATEGORY_ID=@category_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@category_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "SUBCATEGORY_ID"://SELECT BY SUBCATEGORY_ID
                    sql = "SELECT * FROM " + dbName + ".PRODUCTS WHERE SUBCATEGORY_ID=@subcategory_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@subcategory_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "PRODUCT_ID"://SELECT BY PRODUCT_ID
                    sql = "SELECT * FROM " + dbName + ".PRODUCTS WHERE PRODUCT_ID=@product_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@product_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "BRAND"://SELECT BY BRAND
                    sql = "SELECT * FROM " + dbName + ".PRODUCTS WHERE BRAND=@brand";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@brand", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "STOCK"://SELECT BY STOCK
                    sql = "SELECT * FROM " + dbName + ".PRODUCTS WHERE STOCK=@stock";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@stock", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "PRICE"://SELECT BY PRICE
                    sql = "SELECT * FROM " + dbName + ".PRODUCTS WHERE PRICE BETWEEN @price1 AND @price2";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@price1", criteriaData1);
                    cmd.Parameters.AddWithValue("@price2", criteriaData2);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "NAME"://SELECT BY NAME
                    sql = "SELECT * FROM " + dbName + ".PRODUCTS WHERE NAME = @name";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@name", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                default://GENERAL SELECT
                    sql = "SELECT * FROM " + dbName + ".PRODUCTS";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;
            }

            return selectTable;
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX----SALES----XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        public bool salesInsert(float price)
        {
            DateTime _date = DateTime.Now;

            string sql = "INSERT INTO " + dbName + ".SALES (`PRICE`, `GENERATION_DATE`) VALUES (@price, @date)";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@date", _date);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool salesDelete(int sale_id)
        {
            string sql = "DELETE FROM " + dbName + ".`SALES` WHERE `SALES`.`SALE_ID` = @sale_id";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;

                cmd.Parameters.AddWithValue("@sale_id", sale_id);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool salesUpdate(int sale_id, float price)
        {
            string sql = "UPDATE " + dbName + ".SALES SET PRICE=@price WHERE SALE_ID = @sale_id";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@sale_id", sale_id);
                cmd.Parameters.AddWithValue("@price", price);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public DataTable salesSelection(string criteria, string criteriaData1, string criteriaData2)
        {
            string sql = "";
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            DataTable selectTable = new DataTable();

            switch (criteria.ToUpper())
            {
                case "SALE_ID"://SELECT BY SALE_ID
                    sql = "SELECT * FROM " + dbName + ".SALES WHERE SALE_ID=@sale_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@sale_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "GENERATION_DATE"://SELECT BY DATE
                    sql = "SELECT * FROM " + dbName + ".SALES WHERE GENERATION_DATE BETWEEN @date1 AND @date2";
                    cmd = new MySqlCommand(sql, conn);

                    DateTime date1 = DateTime.ParseExact(criteriaData1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime date2 = DateTime.ParseExact(criteriaData2, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    cmd.Parameters.AddWithValue("@date1", date1.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@date2", date2.ToString("yyyy-MM-dd"));
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "PRICE"://SELECT BY PRICE
                    sql = "SELECT * FROM " + dbName + ".SALES WHERE PRICE BETWEEN @price1 AND @price2";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@price1", criteriaData1);
                    cmd.Parameters.AddWithValue("@price2", criteriaData2);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                default://GENERAL SELECT
                    sql = "SELECT * FROM " + dbName + ".SALES";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;
            }

            return selectTable;
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX----SALES_DETAIL----XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        public bool sales_detailInsert(int product_id, int sale_id, int amount, float price)
        {
            DateTime _date = DateTime.Now;

            string sql = "INSERT INTO " + dbName + ".SALES_DETAIL (`SALE_ID`, `PRODUCT_ID`, `AMOUNT`, `PRICE`) VALUES (@sale_id, @product_id, @amount, @price)";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@amount", sale_id);
                cmd.Parameters.AddWithValue("@amount", product_id);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@price", price);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool sales_detailCheckDelete(int sale_id, int product_id)
        {
            string sql = "DELETE FROM " + dbName + ".`SALES_DETAIL` WHERE `SALES_DETAIL`.`SALE_ID` = @sale_id AND `SALES_DETAIL`.`PRODUCT_ID` = @product_id";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;

                cmd.Parameters.AddWithValue("@sale_id", sale_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool sales_detailCheckUpdate(int sale_id, int product_id, int amount, float price)
        {
            string sql = "UPDATE " + dbName + ".SALES_DETAIL SET PRICE = @price, AMOUNT = @amount  WHERE SALE_ID = @sale_id AND PRODUCT_ID = @product_id";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@sale_id", sale_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@price", price);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public DataTable sales_detailSelection(string criteria, string criteriaData1, string criteriaData2)
        {
            string sql = "";
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            DataTable selectTable = new DataTable();

            switch (criteria.ToUpper())
            {
                case "SALE_ID/PRODUCT_ID"://SELECT BY SALE_ID AND PRODUCT_ID
                    sql = "SELECT * FROM " + dbName + ".SALES_DETAIL WHERE SALE_ID = @sale_id AND PRODUCT_ID = @product_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@sale_id", criteriaData1);
                    cmd.Parameters.AddWithValue("@product_id", criteriaData2);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "SALE_ID"://SELECT BY SALE_ID
                    sql = "SELECT * FROM " + dbName + ".SALES_DETAIL WHERE SALE_ID = @sale_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@sale_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "PRODUCT_ID"://SELECT BY PRODUCT_ID
                    sql = "SELECT * FROM " + dbName + ".SALES_DETAIL WHERE PRODUCT_ID = @product_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@product_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "AMOUNT"://SELECT BY AMOUNT
                    sql = "SELECT * FROM " + dbName + ".SALES_DETAIL WHERE AMOUNT = @amount";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@amount", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "PRICE"://SELECT BY PRICE
                    sql = "SELECT * FROM " + dbName + ".SALES_DETAIL WHERE PRICE BETWEEN @price1 AND @price2";
                    cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@price1", criteriaData1);
                    cmd.Parameters.AddWithValue("@price2", criteriaData2);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                default://GENERAL SELECT
                    sql = "SELECT * FROM " + dbName + ".SALES_DETAIL";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;
            }

            return selectTable;
        }


        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX----STORAGE_DETAIL----XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        public bool storage_detailInsert(int product_id, int house_id, int stock, string adquisition_date, int min_stock, string name, string brand, float total_price)
        {
            DateTime _date = DateTime.Now;

            string sql = "INSERT INTO " + dbName + ".STORAGE_DETAIL (`PRODUCT_ID`, `HOUSE_ID`, `STOCK`, `ADQUISITION_DATE`, `MIN_STOCK`, `NAME`, `BRAND`, `TOTAL_PRICE`) VALUES (@product_id, @house_id, @stock, @adquisition_date, @min_stock, @name, @brand, @total_price)";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;


                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@house_id", house_id);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@adquisition_date", adquisition_date);
                cmd.Parameters.AddWithValue("@min_stock", min_stock);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@brand", brand);
                cmd.Parameters.AddWithValue("@total_price", total_price);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool storage_detailDelete(int product_id, int house_id)
        {
            string sql = "DELETE FROM " + dbName + ".`STORAGE_DETAIL` WHERE `STORAGE_DETAIL`.`PRODUCT_ID` = @product_id AND `STORAGE_DETAIL`.`HOUSE_ID` = @house_id";

            try
            {

                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans;

                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@house_id", house_id);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public bool storage_detailUpdate(int product_id, int house_id, int stock, string adquisition_date, int min_stock, string name, string brand, float total_price)
        {
            string sql = "UPDATE " + dbName + ".STORAGE_DETAIL SET STOCK = @stock, MIN_STOCK = @min_stock, NAME = @name, BRAND = @brand, TOTAL_PRICE = @total_price  WHERE PRODUCT_ID = @product_id AND HOUSE_ID = @house_id";

            try
            {
                trans = conn.BeginTransaction();
                cmd = new MySqlCommand(sql, conn);
                cmd.Transaction = trans; cmd = new MySqlCommand(sql, conn);


                cmd.Parameters.AddWithValue("@house_id", house_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@min_stock", min_stock);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@brand", brand);
                cmd.Parameters.AddWithValue("@total_price", total_price);

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                trans.Rollback();
                return false;
            }
        }

        public DataTable storage_detailSelection(string criteria, string criteriaData1, string criteriaData2)
        {
            string sql = "";
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            DataTable selectTable = new DataTable();

            switch (criteria.ToUpper())
            {
                case "HOUSE_ID/PRODUCT_ID"://SELECT BY HOUSE_ID AND PRODUCT_ID
                    sql = "SELECT * FROM " + dbName + ".STORAGE_DETAIL WHERE PRODUCT_ID = @product_id AND HOUSE_ID = @house_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@house_id", criteriaData1);
                    cmd.Parameters.AddWithValue("@product_id", criteriaData2);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "HOUSE_ID"://SELECT BY HOUSE_ID
                    sql = "SELECT * FROM " + dbName + ".STORAGE_DETAIL WHERE HOUSE_ID = @house_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@house_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "PRODUCT_ID"://SELECT BY PRODUCT_ID
                    sql = "SELECT * FROM " + dbName + ".STORAGE_DETAIL WHERE PRODUCT_ID = @product_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@product_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "STOCK"://SELECT BY STOCK
                    sql = "SELECT * FROM " + dbName + ".STORAGE_DETAIL WHERE STOCK = @stock";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@stock", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "NAME"://SELECT BY STOCK
                    sql = "SELECT * FROM " + dbName + ".STORAGE_DETAIL WHERE NAME = @name";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "BRAND"://SELECT BY BRAND
                    sql = "SELECT * FROM " + dbName + ".STORAGE_DETAIL WHERE BRAND = @brand";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@brand", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "TOTAL_PRICE"://SELECT BY TOTAL_PRICE
                    sql = "SELECT * FROM " + dbName + ".STORAGE_DETAIL WHERE TOTAL_PRICE = @total_price";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@total_price", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "ADQUISITION_DATE"://SELECT BY DATE
                    sql = "SELECT * FROM " + dbName + ".STORAGE_DETAIL WHERE ADQUISITION_DATE BETWEEN @date1 AND @date2";
                    cmd = new MySqlCommand(sql, conn);

                    DateTime date1 = DateTime.ParseExact(criteriaData1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime date2 = DateTime.ParseExact(criteriaData2, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    cmd.Parameters.AddWithValue("@date1", date1.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@date2", date2.ToString("yyyy-MM-dd"));
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;


                default://GENERAL SELECT
                    sql = "SELECT * FROM " + dbName + ".STORAGE_DETAIL";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;
            }

            return selectTable;
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX----STORAGE----XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        public DataTable storageSelection(string criteria, string criteriaData1, string criteriaData2)
        {
            string sql = "";
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            DataTable selectTable = new DataTable();

            switch (criteria.ToUpper())
            {
                case "USER_ID/HOUSE_ID"://SELECT BY USER_ID AND HOUSE_ID
                    sql = "SELECT * FROM " + dbName + ".STORAGE WHERE USER_ID = @user_id AND HOUSE_ID = @house_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", criteriaData1);
                    cmd.Parameters.AddWithValue("@house_id", criteriaData2);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "HOUSE_ID"://SELECT BY HOUSE_ID
                    sql = "SELECT * FROM " + dbName + ".STORAGE WHERE HOUSE_ID = @house_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@house_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;

                case "USER_ID"://SELECT BY USER_ID
                    sql = "SELECT * FROM " + dbName + ".STORAGE WHERE USER_ID = @user_id";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", criteriaData1);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;


                default://GENERAL SELECT
                    sql = "SELECT * FROM " + dbName + ".STORAGE";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    selectTable.Load(rdr);
                    rdr.Close();
                    break;
            }

            return selectTable;
        }



        public List<String> getLoginInfo(string username)
        {
            string sql = "";
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            List<String> infoList = new List<String>();

            sql = "SELECT USERNAME, PWD, CREATION_DATE, ROL FROM " + dbName + ".USERS WHERE USERNAME = @user_id";

            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@user_id", username);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                infoList.Add(rdr["USERNAME"].ToString());
                infoList.Add(rdr["PWD"].ToString());
                infoList.Add(rdr["CREATION_DATE"].ToString());
                infoList.Add(rdr["ROL"].ToString());
            }
            rdr.Close();

            return infoList;
        }

        public string getUserDate(string user_id)
        {
            string sql = "";
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            string date = "";

            sql = "SELECT CREATION_DATE FROM " + dbName + ".USERS WHERE USER_ID = @user_id";

            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@user_id", user_id);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                date = rdr["CREATION_DATE"].ToString();
            }
            rdr.Close();

            return date;
        }

    }
}
