using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HW1
{
    public partial class Default : System.Web.UI.Page
    {
        static bool Cardvalid;
        static bool Datevalid;
        static bool Namevalid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int year = DateTime.Now.Year; year <= DateTime.Now.Year + 2; year++)
                {
                    ListItem item = new ListItem(year.ToString(), year.ToString());
                    YearSelect.Items.Add(item);
                }
            }

        }
        protected void Submit_Click(object sender, EventArgs e)
        {
            if (NameTextBox.Text == "")
            {
                NameTxtRequiredLbl.Text = "Your name on card is required!";
            }
            if (CardNumTextBox.Text == "")
            {
                CardNoRequiredTxtLbl.Text = "Card number is required!";
            }

            ListItem selectedMonth = MonthSelect.Items.FindByValue(MonthSelect.Value);
            ListItem selectedYear = YearSelect.Items.FindByValue(YearSelect.Value);

            if (selectedMonth != null && selectedYear != null)
            {
                CardValidDateLabel.Text = selectedMonth.Text + "/" + selectedYear.Text.Substring(2);
            }
            else
            {
                CardValidDateLabel.Text = "mm/yy"; // Reset the label if month or year is not selected
            }
            if (CVCTextBox.Text == "")
            {
                CvcRequiredTxtLbl.Text = "Card CVC is required!";
            }

            else
            {
               

                string CardNumNoSpace = CardNumTextBox.Text.Replace(" ", "");
                Cardvalid = checkLuhn(CardNumNoSpace);


                // Cardvalid && Datevalid
                // In order to make the program work completely remove  Cardvalid && Datevalid from the if state the program will work perfectly.


                if (Cardvalid && Datevalid && Namevalid && Regex.IsMatch(CVCTextBox.Text, "^([0-9][0-9][0-9])") && CardTypeIcon.Text != "<i class='fa fa-credit-card' style='font-size:24px;color:red'></i>")
                {
                    AlertPanel.CssClass = "alert alert-dismissible alert-success";
                    ErrorLbl.Text = "Your card is Valid";

                    AlertPanel.Visible = true;

                }
                else
                {
                    AlertPanel.Visible = true;
                    AlertPanel.CssClass = "alert alert-dismissible alert-danger";

                    if (!Namevalid)
                    {
                        ErrorLbl.Text = "Name on card is in invalid format!";
                    }
                    else if (CardTypeIcon.Text == "<i class='fa fa-credit-card' style='font-size:24px;color:red'></i>")
                    {
                        ErrorLbl.Text = "Unknown card type";
                    }
                    else if (!Cardvalid)
                    {
                        ErrorLbl.Text = "Card number is not Valid!";
                    }
                    else if (!Datevalid)                                                        //!Datevalid
                    {
                        ErrorLbl.Text = "Date is not Valid!";
                    }
                    else if (!Regex.IsMatch(CVCTextBox.Text, "^([0-9][0-9][0-9])"))
                    {
                        ErrorLbl.Text = "Card CVC is in invalid format!";
                    }
                }
                CvcRequiredTxtLbl.Text = "";
            }
        }
        static bool checkLuhn(String No)
        {
            if (No.Length <= 3)
            {
                No = No.PadLeft(16, '0');
            }

            int nDigits = No.Length;

            int nSum = 0;
            bool isSecond = false;
            for (int i = nDigits - 1; i >= 0; i--)
            {

                int d = No[i] - '0';

                if (isSecond == true)
                    d = d * 2;

                // We add two digits to handle
                // cases that make two digits
                // after doubling
                nSum += d / 10;
                nSum += d % 10;

                isSecond = !isSecond;
            }
            return (nSum % 10 == 0);
        }

        protected void CardNumTextBox_TextChanged(object sender, EventArgs e)
        {
            string Iconcardtype = CheckCardType();
            CardTypeLabel.Text = Iconcardtype.Replace("24", "36");
            if (CardNumTextBox.Text != "")
            {
                CardNoLabel.Text = CardNumTextBox.Text;
                CardNoRequiredTxtLbl.Text = "";
            }
            else
            {
                CardNoLabel.Text = "XXXX XXXX XXXX XXXX";
            }
        }

        protected string CheckCardType()
        {
            if (Regex.IsMatch(CardNumTextBox.Text, "^(4)"))
            {
                return CardTypeIcon.Text = "<i class=\"fa fa-cc-visa\" style=\"font-size:24px\"></i>";
            }
            else if (Regex.IsMatch(CardNumTextBox.Text, "^(5[1-5])"))
            {
                return CardTypeIcon.Text = "<i class=\"fa fa-cc-mastercard\" style=\"font-size:24px\"></i>";
            }
            else if (Regex.IsMatch(CardNumTextBox.Text, "^(35)"))
            {
                return CardTypeIcon.Text = "<i class=\"fa fa-cc-jcb\" style=\"font-size:24px\"></i>";
            }
            else if (Regex.IsMatch(CardNumTextBox.Text, "^(6(?:011|5[0-9][0-9]))"))
            {
                return CardTypeIcon.Text = "<i class=\"fa fa-cc-discover\" style=\"font-size:24px\"></i>";
            }
            else if (Regex.IsMatch(CardNumTextBox.Text, "^( )"))
            {
                return CardTypeIcon.Text = "<i class=\"fa fa-credit-card\" style=\"font-size:24px\"></i>";
            }
            return CardTypeIcon.Text = "<i class='fa fa-credit-card' style='font-size:24px;color:red'></i>";

        }

        public bool ValidateCardDate(string date)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact(date, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                if (parsedDate > DateTime.Now)
                {
                    return true;
                }
            }
            return false;
        }
    
        protected void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (NameTextBox.Text != "")
            {
                if (Regex.IsMatch(NameTextBox.Text, "^[a-zA-Z\\s ]+$") && NameTextBox.Text.Contains(" "))
                {
                    NameOnCardLabel.Text = NameTextBox.Text.ToUpper();
                    NameTxtRequiredLbl.Text = "";
                    Namevalid = true;
                }
                else
                {
                    NameOnCardLabel.Text = NameTextBox.Text.ToUpper();
                    Namevalid = false;
                }
            }
            else
            {
                NameOnCardLabel.Text = "FirstName  LastName";
            }
        }
    }
}