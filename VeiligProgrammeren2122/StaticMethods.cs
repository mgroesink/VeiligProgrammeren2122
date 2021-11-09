using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace VeiligProgrammeren2122
{
    /// <summary>
    /// 
    /// </summary>
    public static class StaticMethods
    {

        private readonly static SqlConnection conn =
    new SqlConnection("Server=sql6011.site4now.net;Database=DB_A2A0BC_vp;);

        #region Encryption en decryption methods
        /// <summary>
        /// Encrypts text with numeric key.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string ShiftCypher(string text, int key)
        {
            //TODO: Update this method so that only letters are shifted
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string encryptedText = "";
            foreach (char c in text)
            {
                int oldposition = alphabet.IndexOf(c);
                int newposition = oldposition + key;
                char newCharacter = alphabet[newposition % 52];
                if (oldposition >= 0)
                    encryptedText += newCharacter.ToString();
                else
                    encryptedText += c.ToString();
            }
            return encryptedText;
        }

        /// <summary>
        /// Encrypts text with string key.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Key should only contain letters</exception>
        public static string ShiftCypher(string text, string key)
        {
            //TODO: Encrypt text by using a non-numeric key
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            if (!isValidKey(key))
            {
                throw new ArgumentException("Key should only contain letters");
            }
            string encryptedText = "";
            string filledKey = FillKey(key, text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                int position = alphabet.IndexOf(text[i]);
                if (position >= 0)
                {
                    int keyvalue = position + alphabet.IndexOf(filledKey[i]) + 1;
                    encryptedText += alphabet[keyvalue % 52];
                }
                else
                {
                    encryptedText += text[i];
                }
            }
            return encryptedText;
        }

        /// <summary>
        /// Decrypts text with numeric key.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string UnShiftCypher(string encryptedText, int key)
        {
            string decryptedText = "";
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            //TODO: Insert your code to decrypt the encrypted text using the given key
            // Only letters should be unshifted
            foreach (char c in encryptedText)
            {
                int oldposition = alphabet.IndexOf(c);
                int newposition = oldposition - key;
                char newCharacter = alphabet[(newposition + 52) % 52];
                if (oldposition >= 0)
                    decryptedText += newCharacter.ToString();
                else
                    decryptedText += c.ToString();
            }
            return decryptedText;
        }

        /// <summary>
        /// Decrypts text with key.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Key should only contain letters</exception>
        public static string UnShiftCypher(string text, string key)
        {
            //TODO: Encrypt text by using a non-numeric key
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            if (!isValidKey(key))
            {
                throw new ArgumentException("Key should only contain letters");
            }
            string decryptedText = "";
            string filledKey = FillKey(key, text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                int position = alphabet.IndexOf(text[i]);
                if (position >= 0)
                {
                    int keyvalue = position - alphabet.IndexOf(filledKey[i]) - 1 + 52;
                    decryptedText += alphabet[keyvalue % 52];
                }
                else
                {
                    decryptedText += text[i];
                }
            }
            return decryptedText;
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Checks the login.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Connecting to database failed</exception>
        public static bool CheckLogin(string username, string password)
        {
            bool ok = false;
            SqlCommand cmd = new SqlCommand();
            string sql = "SELECT * FROM Users WHERE UserId = '";
            sql += username + "' AND password = '" + password + "'";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ok = reader.HasRows;
            }
            catch
            {
                throw new Exception("Connecting to database failed");
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return ok;
        }

        public static DataTable GetResults(string studentnumber)
        {
            DataTable result = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Cijfers WHERE UserId = '" + studentnumber + "'";
            cmd.Connection = conn;
            try
            {
                conn.Open();
                result.Load(cmd.ExecuteReader());
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="newpassword">The newpassword.</param>
        /// <param name="confirmpassword">The confirmpassword.</param>
        /// <returns></returns>
        public static string ChangePassword(string username, string password,
            string newpassword, string confirmpassword)
        {

            if (!CheckLogin(username, password))
            {
                return "username and/or old password incorrect";
            }
            else if (newpassword != confirmpassword)
            {
                return "Passwords do not match";
            }
            else
            {
                if (IsValidPassword(newpassword, 8, 2, 2, 1, 1, "+=%!?-_"))
                {
                    // Update the password
                    // SqlCommand cmd2 =
                    //    new SqlCommand("UPDATE Users SET Password = @PASSWORD WHERE Username = @USERNAME",
                    //    conn);
                    SqlCommand cmd2 =
                        new SqlCommand("UPDATE Users SET Password = '" +
                        newpassword + "' WHERE Username = '" +
                        username + "'", conn);

                    //cmd2.Parameters.AddWithValue("@USERNAME", username);
                    //cmd2.Parameters.AddWithValue("@PASSWORD", newpassword);
                    conn.Open();
                    if (cmd2.ExecuteNonQuery() == 0)
                    {
                        return "Updating password failed";
                    }
                    else
                    {
                        return "Password updated successfully";
                    }
                }
            }
            return "New password does not meet criteria";
        }

        /// <summary>
        /// Determines whether [is valid password] [the specified password].
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="minUpper">The minimum upper.</param>
        /// <param name="minLower">The minimum lower.</param>
        /// <param name="minDigit">The minimum digit.</param>
        /// <param name="minSpecial">The minimum special.</param>
        /// <param name="allowedSpecials">The allowed specials.</param>
        /// <returns>
        ///   <c>true</c> if [is valid password] [the specified password]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidPassword(string password, int minLength,
            int minUpper, int minLower, int minDigit, int minSpecial, string allowedSpecials)
        {
            int upperCount = 0;
            int lowerCount = 0;
            int digitCount = 0;
            int specialsCount = 0;
            foreach (char c in password)
            {
                int ascii = (int)c;
                if (ascii >= 65 && ascii <= 90) upperCount++;
                if (ascii >= 97 && ascii <= 122) lowerCount++;
                if (ascii >= 48 && ascii <= 57) digitCount++;
                if (allowedSpecials.Contains(c)) specialsCount++;
            }
            return upperCount >= minUpper && lowerCount >= minLower &&
                digitCount >= minDigit && specialsCount >= minSpecial &&
                password.Length >= minLength;
        }

        /// <summary>
        /// Creates random student results.
        /// </summary>
        /// <param name="schooljaar">The schooljaar.</param>
        /// <param name="periode">The periode.</param>
        public static void CreateStudentResults(string schooljaar, byte periode)
        {
            Random rnd = new Random();
            DataTable students = new DataTable();
            DataTable courses = new DataTable();
            SqlCommand cmdStudents = new SqlCommand("SELECT * FROM Users", conn);
            SqlCommand cmdCourses = new SqlCommand("SELECT * FROM VAK", conn);
            SqlCommand cmdResult = new SqlCommand("", conn);
            conn.Open();
            students.Load(cmdStudents.ExecuteReader());
            courses.Load(cmdCourses.ExecuteReader());
            conn.Close();
            foreach (DataRow student in students.Rows)
            {
                foreach (DataRow course in courses.Rows)
                {
                    int points = rnd.Next(1, 10);
                    conn.Open();
                    cmdResult.CommandText =
                        "INSERT INTO Cijfers(UserId , Vakcode , Resultaat , Schooljaar , Periode)" +
                        " VALUES('" + student[2] + "' , '" + course[0] + "' , " +
                        points.ToString() + ", '" + schooljaar + "', " +
                        periode + ")";
                    cmdResult.ExecuteReader();
                    conn.Close();
                }
            }
            conn.Close();
        }

        /// <summary>
        /// Fills the key to a given length.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        private static string FillKey(string key, int length)
        {
            string filled = key;
            while (filled.Length < length)
            {
                filled += key;
            }

            return filled.Substring(0, length);
        }

        /// <summary>
        /// Determines whether [is valid key] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if [is valid key] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        private static bool isValidKey(string key)
        {
            foreach (var c in key)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }
            return true;
        } 
        #endregion
    }
}
