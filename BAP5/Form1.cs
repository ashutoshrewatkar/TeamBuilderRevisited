using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BAP5
{
    public partial class TeamBuilder2Form : Form
    {
        decimal[,] MealPriceWRTEvent =
        {
            {99m, 75m, 24m, 0m },
            {149m, 113m, 36m, 0m },
            {198m, 150m, 48m, 0m },
            {297m, 225m, 72m, 0m },
            {99m, 75m, 25m, 0m },
            {248m, 188m, 60m, 0m },
            {149m, 113m, 36m, 0m },
            {99m, 75m, 24m, 0m },
            {297m, 225m, 72m, 0m },
            {248m, 188m, 60m, 0m }
        };

        int[,] AvailableSpots =
        {
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 }
        };


        string[] EventTitle = { "Murder Mystery Weekend", "CSI Weekend", "The Great Outdoors", "The Chase", "Digital Refresh", "Action Photography", "Team Ryder Cup", "Abseiling", "War Games", "Find Wally" };
        decimal[] EventRegFee = { 600.00m, 1000.00m, 1500.00m, 1800.00m, 599.00m, 999.00m, 619.00m, 499.00m, 1999.00m, 799.00m };
        int[] EventDays = { 2, 3, 4, 6, 2, 5, 3, 2, 6, 5 };
        string[] Locations = { "Cork", "Dublin", "Galway", "Belmullet", "Belfast" };
        decimal[] LodgingFee = { 250.00m, 165.00m, 225.00m, 305.00m, 95.00m };

        string path2 = @"C:\Users\Acer\source\repos\BAP5\BAP5\bin\Debug\AvailableSpots.txt";
        string path3 = @"C:\Users\Acer\source\repos\BAP5\BAP5\bin\Debug\AvailableSpots_Readable.txt";
        string path1;
        string SelectedEvent;
        string SelectedLocation;
        string SelectedMealPlan;
        string DateToday;
        string TimeNow;
        int SelectedEventDays;
        int NumberOfTickets;
        int SelectedAvailableSpots;
        string TransactionNumber;
        decimal EventRegCost;
        decimal LodgingCost;
        decimal TotalCost;
        decimal SelectedMealPlanPrice;
        decimal TotalOrderCost;
        StreamWriter OutputFile;

        bool x = true;

        int EventsListViewIndex;
        int LocationListBoxIndex;

        //making a list
        List<string> ListTransactionNumber = new List<string>();
        List<string> ListSelectedEvent = new List<string>();
        List<decimal> ListSelectedEventPrice = new List<decimal>();
        List<string> ListSelectedLocation = new List<string>();
        List<string> ListSelectedMealPlan = new List<string>();
        List<decimal> ListTotalCost = new List<decimal>();
        List<int> ListQuantity = new List<int>();

        public TeamBuilder2Form()
        {
            InitializeComponent();
        }



        private void EventsListView_MouseClick_1(object sender, MouseEventArgs e)
        {
            try
            {
                //Enable stuff
                LocationListBox.Enabled = true;

                //storing in variables
                SelectedEvent = EventTitle[EventsListView.SelectedItems[0].Index];
                SelectedEventDays = EventDays[EventsListView.SelectedItems[0].Index];

                EventDisplayLabel.Text = SelectedEvent;
                EventRegCost = EventRegFee[EventsListView.SelectedItems[0].Index];
                TotalCost = EventRegCost;
                NumberOfTickets = int.Parse(NumberOfTicketsTextBox.Text);
                TotalCost += (((SelectedEventDays * LodgingCost) + SelectedMealPlanPrice) * NumberOfTickets);
                TotalCostDisplayLabel.Text = TotalCost.ToString("C");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Entry Error!", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void LocationListBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //Enable stuff
                MealPlansGroupBox.Enabled = true;

                //take location input
                SelectedLocation = Locations[LocationListBox.SelectedIndex];
                LocationDisplayLabel.Text = SelectedLocation;

                LodgingCost = LodgingFee[LocationListBox.SelectedIndex] * SelectedEventDays;

                //Display Total Cost
                NumberOfTickets = int.Parse(NumberOfTicketsTextBox.Text);
                TotalCost = (((EventRegCost + LodgingCost) + SelectedMealPlanPrice) * NumberOfTickets);
                TotalCostDisplayLabel.Text = TotalCost.ToString("c");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Entry Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void NoneRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            SelectedMealPlan = NoneRadioButton.Text;
            SelectedMealPlanPrice = MealPriceWRTEvent[(EventsListView.SelectedItems[0].Index), 3];
            MealPlanDisplayLabel.Text = "None: " + SelectedMealPlanPrice.ToString("c");

            NumberOfTickets = int.Parse(NumberOfTicketsTextBox.Text);
            TotalCost = ((EventRegCost + LodgingCost + SelectedMealPlanPrice) * NumberOfTickets);
            TotalCostDisplayLabel.Text = TotalCost.ToString("c");
            BookButton.Enabled = true;
        }

        private void FullBoardMealRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            SelectedMealPlan = FullBoardMealRadioButton.Text;
            SelectedMealPlanPrice = MealPriceWRTEvent[(EventsListView.SelectedItems[0].Index), 0];
            MealPlanDisplayLabel.Text = "Full Board: " + SelectedMealPlanPrice.ToString("c");

            //total cost
            NumberOfTickets = int.Parse(NumberOfTicketsTextBox.Text);
            TotalCost = ((EventRegCost + LodgingCost + SelectedMealPlanPrice) * NumberOfTickets);
            TotalCostDisplayLabel.Text = TotalCost.ToString("c");
            BookButton.Enabled = true;
        }

        private void HalfBoardMealRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            SelectedMealPlan = HalfBoardMealRadioButton.Text;
            SelectedMealPlanPrice = MealPriceWRTEvent[(EventsListView.SelectedItems[0].Index), 1];
            MealPlanDisplayLabel.Text = "Half Board: " + SelectedMealPlanPrice.ToString("c");

            //total cost
            NumberOfTickets = int.Parse(NumberOfTicketsTextBox.Text);
            TotalCost = ((EventRegCost + LodgingCost + SelectedMealPlanPrice) * NumberOfTickets);
            TotalCostDisplayLabel.Text = TotalCost.ToString("c");
            BookButton.Enabled = true;
        }

        private void BreakfastMealRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            SelectedMealPlan = BreakfastMealRadioButton.Text;
            SelectedMealPlanPrice = MealPriceWRTEvent[(EventsListView.SelectedItems[0].Index), 2];
            MealPlanDisplayLabel.Text = "Full Board: " + SelectedMealPlanPrice.ToString("c");

            //total cost
            NumberOfTickets = int.Parse(NumberOfTicketsTextBox.Text);
            TotalCost = ((EventRegCost + LodgingCost + SelectedMealPlanPrice) * NumberOfTickets);
            TotalCostDisplayLabel.Text = TotalCost.ToString("c");
            BookButton.Enabled = true;
        }

        private void NumberOfTicketsTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                NumberOfTickets = int.Parse(NumberOfTicketsTextBox.Text);
                TotalCost = ((EventRegCost + LodgingCost + SelectedMealPlanPrice) * NumberOfTickets);
                TotalCostDisplayLabel.Text = TotalCost.ToString("c");
            }
            catch(Exception ex1)
            {
                MessageBox.Show(ex1.Message, "Data Entry Error!", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    
            }

        }

        private void BookButton_Click(object sender, EventArgs e)
        {
            //Disable book button
            BookButton.Enabled = false;
            LocationListBox.Enabled = false;
            MealPlansGroupBox.Enabled = false;
            BookingInfoLabel.Visible = true;

            if (x == true)
            {
                AvailableSpotsArray();
                x = false;
            }

            int EventsListViewIndex = EventsListView.SelectedItems[0].Index;
            int LocationListBoxIndex = LocationListBox.SelectedIndex;
            SelectedAvailableSpots = AvailableSpots[EventsListViewIndex, LocationListBoxIndex];
            NumberOfTickets = int.Parse(NumberOfTicketsTextBox.Text);
            if (NumberOfTickets > 0)
            {
                if (NumberOfTickets <= SelectedAvailableSpots)
                {
                    TransactionNumberGenerate();
                    MessageBox.Show("Transaction Number: " + TransactionNumber + "\nEvent: " + SelectedEvent + "\nLocation:" + SelectedLocation + "\nMeal Plan: " + SelectedMealPlan + "\nTotal Cost: " + TotalCost + "\nQuantity: " + NumberOfTickets, "Added to bookings!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    AvailableSpots[EventsListViewIndex, LocationListBoxIndex] = SelectedAvailableSpots - NumberOfTickets;

                    //storing in list
                    ListTransactionNumber.Add(TransactionNumber);
                    ListSelectedEvent.Add(SelectedEvent);
                    ListSelectedEventPrice.Add(EventRegCost);
                    ListSelectedLocation.Add(SelectedLocation);
                    ListSelectedMealPlan.Add(SelectedMealPlan);
                    ListTotalCost.Add(TotalCost);
                    ListQuantity.Add(NumberOfTickets);

                    CompleteOrderButton.Enabled = true;

                    //reset form state
                    ResetDetailsEntered();

                }

                else
                {
                    MessageBox.Show("Enter a number lower than or equal to " + SelectedAvailableSpots);
                    NumberOfTicketsTextBox.Focus();
                    BookButton.Enabled = true;
                }

            }

            else
            {
                MessageBox.Show("Enter a number lower than or equal to " + SelectedAvailableSpots);
                NumberOfTicketsTextBox.Focus();
                NumberOfTicketsTextBox.Text = string.Empty;
                BookButton.Enabled = true;
            }
        }

        public void TransactionNumberGenerate()
        {
            //Generating random numbers
            Random RandomNumber = new Random();
            TransactionNumber = (RandomNumber.Next(100000, 999999)).ToString();
        }

        private void CompleteOrderButton_Click(object sender, EventArgs e)
        {
            //visibility
            BookingListView.Visible = true;
            UserSummaryDataGroupBox.Visible = false;
            TotalOrderCostDisplayLabel.Visible = true;
            TotalOrderCostLabel.Visible = true;
            

            //Displaying in List
            BookingListView.Items.Clear();
            for (int i = 0; i < ListTransactionNumber.Count; i++)
            {
                ListViewItem BookingList = new ListViewItem(ListTransactionNumber[i].ToString());
                BookingList.SubItems.Add(ListSelectedEvent[i]);
                BookingList.SubItems.Add(ListSelectedEventPrice[i].ToString());
                BookingList.SubItems.Add(ListSelectedLocation[i]);
                BookingList.SubItems.Add(ListSelectedMealPlan[i]);
                BookingList.SubItems.Add(ListTotalCost[i].ToString());
                BookingList.SubItems.Add(ListQuantity[i].ToString());
                BookingListView.Items.Add(BookingList);
                TotalOrderCost += ListTotalCost[i];
               

                WriteAvailableSpots();
            }
            TotalOrderCostDisplayLabel.Text = TotalOrderCost.ToString();

            


            //writing transaction data to a file
            path1 = @"C:\Users\Acer\source\repos\BAP5\BAP5\bin\Debug\" + DateToday + ".txt";
            if (DateToday == DateTime.Today.ToString("dd-MM-yyyy") && File.Exists(path1))
            {
                OutputFile.Close();
                WriteDataToFile();
            }
            else
            {
                using (OutputFile = File.CreateText(path1))
                {
                    OutputFile.Close();
                    WriteDataToFile();
                }
            }
            ResetDetailsEntered();
            //clear list 
            ListTransactionNumber.Clear();
            ListSelectedEvent.Clear();
            ListSelectedEventPrice.Clear();
            ListSelectedLocation.Clear();
            ListSelectedMealPlan.Clear();
            ListTotalCost.Clear();
            ListQuantity.Clear();
        }

        public void ClearButton_Click(object sender, EventArgs e)
        {

            BookingListView.Visible = false;
            UserSummaryDataGroupBox.Visible = true;
            TotalOrderCostDisplayLabel.Visible = false;
            TotalOrderCostLabel.Visible = false;
            CompleteOrderButton.Enabled = false;
            TotalOrderCost = 0;

            ResetDetailsEntered();

            //clear transaction list
            ListTransactionNumber.Clear();
            ListSelectedEvent.Clear();
            ListSelectedEventPrice.Clear();
            ListSelectedLocation.Clear();
            ListSelectedMealPlan.Clear();
            ListTotalCost.Clear();
            ListQuantity.Clear();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            AvailableSpots_Readability();
            Application.Exit();
        }
        public void AvailableSpotsArray()
        {
            var InputFile = File.OpenText(path2);
            while (!InputFile.EndOfStream)
            {
                for (int i = 0; i < 10; i++)
                {
                    InputFile.ReadLine();
                    for (int j = 0; j < 5; j++)
                    {
                        AvailableSpots[i, j] = int.Parse(InputFile.ReadLine());
                    }

                }
            }
            InputFile.Close();
        }

        public void WriteDataToFile()
        {


            for (int i = 0; i < ListTransactionNumber.Count; i++)
            {
                OutputFile = File.AppendText(path1);
                //writeline
                OutputFile.WriteLine("----------------------");
                OutputFile.WriteLine(ListTransactionNumber[i].ToString());
                OutputFile.WriteLine(ListSelectedEvent[i]);
                OutputFile.WriteLine(ListSelectedEventPrice[i].ToString());
                OutputFile.WriteLine(ListSelectedLocation[i]);
                OutputFile.WriteLine(ListSelectedMealPlan[i]);
                OutputFile.WriteLine(ListTotalCost[i].ToString());
                OutputFile.WriteLine(ListQuantity[i].ToString());
                //close the file
                OutputFile.Close();

            }

        }

        private void AvailableSpots_Readability()
        {
            DateToday = DateTime.Today.ToString("dd-MM-yyyy");

            var time = DateTime.Now;
            TimeNow = time.ToString("hh:mm:ss");
            AvailableSpotsArray();
            try
            {
                File.WriteAllText(path3, string.Empty);
                StreamWriter OutputFileSpots = File.AppendText(path3);
                //writelines
                OutputFileSpots.WriteLine("\t\t\t\t\t\t\t\t{0}\t\t{1}\t\t{2}\t\t{3}\t\t{4}", Locations[0], Locations[1], Locations[2], Locations[3], Locations[4]);
                OutputFileSpots.WriteLine("{0}\t\t\t{1}\t\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t\t{5}", EventTitle[0], AvailableSpots[0, 0], AvailableSpots[0, 1], AvailableSpots[0, 2], AvailableSpots[0, 3], AvailableSpots[0, 4]);
                OutputFileSpots.WriteLine("{0}\t\t\t\t\t\t{1}\t\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t\t{5}", EventTitle[1], AvailableSpots[1, 0], AvailableSpots[1, 1], AvailableSpots[1, 2], AvailableSpots[1, 3], AvailableSpots[1, 4]);
                OutputFileSpots.WriteLine("{0}\t\t\t\t{1}\t\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t\t{5}", EventTitle[2], AvailableSpots[2, 0], AvailableSpots[2, 1], AvailableSpots[2, 2], AvailableSpots[2, 3], AvailableSpots[2, 4]);
                OutputFileSpots.WriteLine("{0}\t\t\t\t\t\t{1}\t\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t\t{5}", EventTitle[3], AvailableSpots[3, 0], AvailableSpots[3, 1], AvailableSpots[3, 2], AvailableSpots[3, 3], AvailableSpots[3, 4]);
                OutputFileSpots.WriteLine("{0}\t\t\t\t\t{1}\t\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t\t{5}", EventTitle[4], AvailableSpots[4, 0], AvailableSpots[4, 1], AvailableSpots[4, 2], AvailableSpots[4, 3], AvailableSpots[4, 4]);
                OutputFileSpots.WriteLine("{0}\t\t\t\t{1}\t\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t\t{5}", EventTitle[5], AvailableSpots[5, 0], AvailableSpots[5, 1], AvailableSpots[5, 2], AvailableSpots[5, 3], AvailableSpots[5, 4]);
                OutputFileSpots.WriteLine("{0}\t\t\t\t\t{1}\t\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t\t{5}", EventTitle[6], AvailableSpots[6, 0], AvailableSpots[6, 1], AvailableSpots[6, 2], AvailableSpots[6, 3], AvailableSpots[6, 4]);
                OutputFileSpots.WriteLine("{0}\t\t\t\t\t\t{1}\t\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t\t{5}", EventTitle[7], AvailableSpots[7, 0], AvailableSpots[7, 1], AvailableSpots[7, 2], AvailableSpots[7, 3], AvailableSpots[7, 4]);
                OutputFileSpots.WriteLine("{0}\t\t\t\t\t\t{1}\t\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t\t{5}", EventTitle[8], AvailableSpots[8, 0], AvailableSpots[8, 1], AvailableSpots[8, 2], AvailableSpots[8, 3], AvailableSpots[8, 4]);
                OutputFileSpots.WriteLine("{0}\t\t\t\t\t\t{1}\t\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t\t{5}", EventTitle[9], AvailableSpots[9, 0], AvailableSpots[9, 1], AvailableSpots[9, 2], AvailableSpots[9, 3], AvailableSpots[9, 4]);
                OutputFileSpots.WriteLine();
                OutputFileSpots.WriteLine("Date: " + DateToday);
                OutputFileSpots.WriteLine("Time: " + TimeNow);
                OutputFileSpots.Close();
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteSpots_Click(object sender, EventArgs e)
        {
            AvailableSpots_Readability();
        }


        public void WriteAvailableSpots()
        {

            //storing remaining spots in given array
            File.WriteAllText(path2, string.Empty);
            OutputFile = File.AppendText(path2);
            for (int i = 0; i < 10; i++)
            {
                OutputFile.WriteLine();
                for (int j = 0; j < 5; j++)
                {
                    OutputFile.WriteLine(AvailableSpots[i, j]);
                }

            }
            OutputFile.Close();
        }

        public void ResetDetailsEntered()
        {
            EventsListView.Items[0].Selected = false;
            LocationListBox.SelectedIndex = -1;
            NoneRadioButton.Checked = false;
            HalfBoardMealRadioButton.Checked = false;
            FullBoardMealRadioButton.Checked = false;
            BreakfastMealRadioButton.Checked = false;
            NumberOfTicketsTextBox.Text = "1";
            EventDisplayLabel.Text = string.Empty;
            MealPlanDisplayLabel.Text = string.Empty;
            LocationDisplayLabel.Text = string.Empty;
            TotalCostDisplayLabel.Text = string.Empty;

            
        }

        private void AvailableSpotsReloadButton_Click(object sender, EventArgs e)
        {
           var confirmation =  MessageBox.Show("Are you sure you want to reload the initial spots?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmation == DialogResult.Yes)
            {
                File.Delete("AvailableSpots.txt");
                File.Copy("AvailableSpots_backup.txt", "AvailableSpots.txt");
                MessageBox.Show("Initial spots written in AvailableSpots.txt","Done",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
          
        }
    }
}

