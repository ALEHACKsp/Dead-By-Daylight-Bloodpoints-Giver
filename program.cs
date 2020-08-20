/*
	Give yourself any amount of bloodpoints on Dead By Daylight.
	Created by Spencer#0003 on Discord.

	If anyone from Behaviour Interactive is looking at this, please fix your fucking security.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadByDaylight
{
    public partial class Form1 : Form
    {
        private int amountToGive;

        public Form1()
        {
            InitializeComponent();
        }

		private string doTheTostring()
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://steam.live.bhvrdbd.com/api/v1/extensions/rewards/grantCurrency/");
			httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Accept = "*/*";
			httpWebRequest.Headers["Accept-Encoding"] = "deflate, gzip";
			httpWebRequest.UserAgent = "DeadByDaylight/++DeadByDaylight+Live-CL-281719 Windows/10.0.18363.1.256.64bit"; //I am 100% the game
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Headers["x-kraken-client-platform"] = "steam";
			httpWebRequest.Headers["x-kraken-client-provider"] = "steam";
			Cookie cookie = new Cookie();
			cookie.Name = "bhvrSession";
			cookie.Value = cookieBox.Text;
			cookie.Domain = "steam.live.bhvrdbd.com";
			httpWebRequest.CookieContainer = new CookieContainer();
			httpWebRequest.CookieContainer.Add(cookie);
			using (Stream requestStream = httpWebRequest.GetRequestStream())
			{
				using (StreamWriter streamWriter = new StreamWriter(requestStream))
				{
					streamWriter.Write("{\"data\":{\"rewardType\":\"Story\",\"walletToGrant\":{\"balance\":" + Convert.ToString(amountToGive) + ",\"currency\":\"Bloodpoints\"}}}");
				};
			};
			string result;
			using (WebResponse response = httpWebRequest.GetResponse())
			{
				using (Stream responseStream = response.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			return result;
		}

		private void applyBtn_Click(object sender, EventArgs e)
		{
			//Check cookie

			if (cookieBox.Text == "")
			{
				MessageBox.Show("Oi dumb fuck, put your fucking token in.");
				return;
			};

			//Give Bloodpoints

			if (bloodpointsCheckbox.Checked)
			{
				try
				{
					amountToGive = int.Parse(bloodpointsText.Text);
				}
				catch (Exception)
				{
					MessageBox.Show("Invalid integer.");
				};

				if (amountToGive > 1000000 || amountToGive <= 0)
				{
					MessageBox.Show("Amount must be less than 1,000,000 and bigger than 0.");
					return;
				};
			};

			doTheTostring();
			MessageBox.Show("You have been granted " + Convert.ToString(amountToGive) + " bloodpoints.");
		}
    };
};
