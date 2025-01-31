﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomerMaster : System.Web.UI.MasterPage
{
    DAL2 dal;
    Bank BankSys1;
    string pin;
    string login;
    decimal withdrawn;
    private Customer loggedInCustomer;
    private Account custAccount;
    int amountwithdraw;
    Boolean transsucc;
    decimal withdrawdec;
    decimal bal;
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["Bank"] == null)
        {
            //if Bank is null then redirect to menu
            Response.Redirect("~/Index.aspx");
        }
        else
        {
            //retrieving data from the session
            BankSys1 = (Bank)Session["Bank"];
            pin = (string)Session["pin"];
            login = (string)Session["login"];
            Dictionary<string, Customer> dCustomers = BankSys1.getcustomerData();
            foreach (KeyValuePair<String, Customer> c in dCustomers)
            {
                if (login.Equals(c.Key) && (c.Value.checkPin(pin)))
                {
                    //setting customer values
                    loggedInCustomer = c.Value;
                    custAccount = c.Value.getAccount(pin);

                }
            }
            //inserting data into textboxes
            bal = custAccount.getbalance();
            lblBal.Text = bal.ToString("C");
            LblName.Text = loggedInCustomer.getfirstname() + " " + loggedInCustomer.getsurname();
            LblAccNum.Text = custAccount.getaccnumber().ToString();

        }

    }




    protected void lBtnHome_Click(object sender, EventArgs e)
    {

    }

    protected void lBtnBalance_Click(object sender, EventArgs e)
    {
        WithdrawEuro.Checked = false;
        lblError.Text = "";
        //displaying and hiding relevant info
        ContBut.Visible = true;
        ExitBut.Visible = true;
        LblName.Visible = true;
        LblAccNum.Visible = true;
        lblBal.Visible = true;
        lblBalRecNew.Visible = false;
        lblBalWithdrawRec.Visible = false;
        ContButRec.Visible = false;
        ExitButRec.Visible = false;
        lblReceiptHeader.Visible = false;
        WithdrawList.Visible = false;
        TxtWithdraw.Visible = false;
        WithdrawEuro.Visible = false;
        ContBut.Visible = false;
        ExitBut.Visible = false;
        ContButRec.Visible = false;
        ExitButRec.Visible = false;
    }

    protected void lBtnWithdraw_Click(object sender, EventArgs e)
    {

        WithdrawEuro.Checked = false;
        lblError.Text = "";
        //displaying and hiding relevant info
        ContBut.Visible = true;
        ExitBut.Visible = true;
        LblName.Visible = true;
        LblAccNum.Visible = true;
        lblBal.Visible = true;
        WithdrawList.Visible = true;
        TxtWithdraw.Visible = true;
        WithdrawEuro.Visible = true;
        lblBalRecNew.Visible = false;
        lblBalWithdrawRec.Visible = false;
        ContButRec.Visible = false;
        ExitButRec.Visible = false;
        lblReceiptHeader.Visible = false;
    }

    protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void ExitBut_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        WithdrawEuro.Checked = false;
        //sends the user to main menu 
        Response.Redirect("~/Index.aspx");
    }

    protected void ContBut_Click(object sender, EventArgs e)
    {


        lblError.Text = "";
        if (TxtWithdraw.Text.Equals(""))
        {
            //converts the select value in checked box too integer and stores
            amountwithdraw = Convert.ToInt32(WithdrawList.SelectedValue);
        }
        else
        {
            //converts the entered value in text box too integer and stores
            amountwithdraw = Convert.ToInt32(TxtWithdraw.Text);
        }

        if (amountwithdraw % 10 != 0)
        {
            //if the value isnt a multiple of 10 then error is displayed
            lblError.Text = "Error- Amount must be a multiple of 10";
        }
        else if (WithdrawEuro.Checked)
        {
            //if chosen to withdraw as euros amount withdraw is divided by exchange rate stored in banksys and stored in withdrawdec
            withdrawdec = amountwithdraw / BankSys1.getexchangerate();
            

            WithdrawEuro.Checked = false;
        }
        else
        {
            //setting amount withdraw to withdraw dec 
            withdrawdec = amountwithdraw;
            //setting the new balance
        }
        transsucc = BankSys1.Withdraw(login, pin, withdrawdec);
        //checking if transaction succeeds
        if (transsucc == false)
        {
            //display error message if it fails
            lblError.Text = "Error - Insufficent Funds";
        }
        else
        {
            bal = custAccount.getbalance();
            //display balance in the  designated label
            lblBal.Text = bal.ToString("C");
        }
    }


    protected void ContButRec_Click(object sender, EventArgs e)
    {

        lblError.Text = "";
        if (TxtWithdraw.Text.Equals(""))
        {
            //converts the select value in checked box too integer and stores
            amountwithdraw = Convert.ToInt32(WithdrawList.SelectedValue);
        }
        else
        {
            //converts the select value in text box too integer and stores
            amountwithdraw = Convert.ToInt32(TxtWithdraw.Text);
        }

        if (amountwithdraw % 10 != 0)
        {
            //if the value isnt a multiple of 10 then error is displayed
            lblError.Text = "Error- Amount must be a multiple of 10";
        }
        else if (WithdrawEuro.Checked)
        {
            //if chosen to withdraw as euros amount withdraw is divided by exchange rate stored in banksys and stored in withdrawdec
            withdrawdec = amountwithdraw / BankSys1.getexchangerate();
            //display balance in the  designated label
            lblBal.Text = bal.ToString("C");
            //setting the new balance
            WithdrawEuro.Checked = false;
        }
        else
        {
            //if chosen to withdraw as euros amount withdraw is divided by exchange rate stored in banksys and stored in withdrawdec
            withdrawdec = amountwithdraw;
            //display balance in the  designated label
            lblBal.Text = bal.ToString("C");
        }
        transsucc = BankSys1.Withdraw(login, pin, withdrawdec);
        if (transsucc == false)
        {
            lblError.Text = "Error - Insufficent Funds";
        }
        else
        {
            bal = custAccount.getbalance();
            //display balance in the  designated labels
            lblBalRecNew.Text = bal.ToString("C");

            lblBalWithdrawRec.Text = withdrawdec.ToString("C");
            
        }
    }
    protected void ExitButRec_Click(object sender, EventArgs e)
    {
        WithdrawEuro.Checked = false;
        lblError.Text = "";
        Response.Redirect("~/Index.aspx");
    }

    protected void lBtnWithdrawRec_Click(object sender, EventArgs e)
    {
        WithdrawEuro.Checked = false;
        lblError.Text = "";
        //displaying and hiding relevant info
        lblReceiptHeader.Visible = true;
        ContBut.Visible = false;
        ExitBut.Visible = false;
        ContButRec.Visible = true;
        ExitButRec.Visible = true;
        LblName.Visible = true;
        LblAccNum.Visible = true;
        lblBal.Visible = true;
        lblBalRecNew.Visible = true;
        lblBalWithdrawRec.Visible = true;
        WithdrawList.Visible = true;
        TxtWithdraw.Visible = true;
        WithdrawEuro.Visible = true;
    }

    protected void WithdrawList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}

