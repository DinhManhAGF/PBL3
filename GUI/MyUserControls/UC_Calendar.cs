using Org.BouncyCastle.Math;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.MyUserControls
{

    public partial class UC_Calendar : UserControl

    {
        int MONTH, YEAR;
        Button[,] btn = new Button[6, 7];
        String[,] dTime = new String[6, 7];
        public UC_Calendar()
        {
            InitializeComponent();
            init();
        }
        //Methods
        #region Methods
        public void init()
        {
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 7; j++)
                {
                    btn[i, j] = new Button();
                    btn[i, j].Font = new Font("Lucida Handwriting", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    btn[i, j].Size = new Size(152, 82);
                    btn[i, j].BackColor = Color.White;
                    btn[i, j].ForeColor = Color.Black;
                    btn[i, j].FlatStyle = FlatStyle.Flat;
                    btn[i, j].FlatAppearance.BorderSize = 1;
                    btn[i, j].Click += buttonDate_Click;
                    flowLayoutPanel1.Controls.Add(btn[i, j]);
                }
        }
        //Kiem tra nam nhuan
        public static bool isLeapYear(BigInteger N)
        {
            if (N.Mod(toBig(4)).CompareTo(toBig(0)) == 0 && N.Mod(toBig(100)).CompareTo(toBig(0)) != 0)
                return true;
            if (N.Mod(toBig(400)).CompareTo(toBig(0)) == 0)
                return true;
            return false;
        }
        //Determine number of day in the month
        public static int Nday(int month, BigInteger year)
        {
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    if (isLeapYear(year))
                        return 29;
                    return 28;
            }
            return 0;
        }

        public static BigInteger toBig(int s)
        {
            return new BigInteger(s.ToString());
        }
        public String leng2(String s)
        {
            if (s.Length == 1)
                return "0" + s;
            return s;
        }
        //Determine thứ
        public static int getThu(int month, BigInteger year)
        {
            year = year.Subtract(toBig(1));
            BigInteger d = year;
            d = year.Divide(toBig(4));
            d = d.Subtract(year.Divide(toBig(100)));
            d = d.Add(year.Divide(toBig(400)));
            d = d.Add(year.Multiply(toBig(365)));
            for (int i = 1; i < month; i++)
                d = d.Add(toBig(Nday(i, year.Add(toBig(1)))));
            //		System.out.println(d);
            d = d.Mod(toBig(7)).Add(toBig(2));
            return Convert.ToInt32(d.ToString());
            //Cong them 2 de lay ra ten thu luon
            //return getDay(month, year) % 7 + 2;
        }
        public void reset()
        {
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 7; j++)
                {
                    btn[i, j].BackColor = Color.White;
                    btn[i, j].ForeColor = Color.White;
                    dTime[i, j] = "";
                }
        }
        public int[,] update(int month, BigInteger year)
        {
            reset();
            int[,] a = new int[6, 7];
            int thu = getThu(month, year);
            int day = Nday(month, year);
            //Previous day
            int pday = 0;
            if (month > 1)
                pday = Nday(month - 1, year);
            else
                pday = Nday(12, year.Subtract(toBig(1)));
            int start = thu - 1;
            if (start == 7)
                start = 0;
            int I = 0, J = start;
            for (int i = 1; i <= day; i++)
            {
                btn[I, J].Text = i.ToString();
                btn[I, J].ForeColor = Color.Black;
                btn[I, J].BackColor = Color.White;
                if (i == DateTime.Now.Day && month == DateTime.Now.Month && year.IntValue == DateTime.Now.Year)
                    btn[I, J].BackColor = Color.GreenYellow;
                dTime[I, J] = leng2(i + "") + "-" + leng2(month + "") + "-" + year;
                if (year.CompareTo(toBig(YEAR)) == 0 && MONTH == month + 1 && i == day)
                {
                    btn[I, J].BackColor = Color.Cyan;
                }
                J++;
                if (J == 7)
                {
                    J = 0;
                    I++;
                }
            }
            //Determine phần ngày của tháng trước đó
            for (int i = start - 1; i >= 0; i--)
            {
                btn[0, i].Text = (pday-- + "");
                btn[0, i].BackColor = Color.Gray;
            }
            //Determine phần ngày của tháng sau đó
            int st = 1;
            while (!(I == 6 && J == 0))
            {
                btn[I, J].Text = (st++ + "");
                btn[I, J].BackColor = Color.Gray;
                J++;
                if (J == 7)
                {
                    J = 0;
                    I++;
                }
            }
            return a;
        }
        private void LoadDays()
        {
            BigInteger Year_new = toBig(YEAR);
            int[,] a = update(MONTH, Year_new);
            int check = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (check == 0 && (j != 0 && a[i, j] < a[i, j - 1]))
                    {
                        check++;
                    }
                    if (check == 1)
                    {
                        if (j != 6)
                        {
                            if (a[i, j] > a[i, j + 1])
                                check++;
                        }
                        else
                        {
                            if (i == 5)
                                check++;
                            else if (a[i, j] > a[i + 1, 0])
                                check++;
                        }
                    }
                }
            }
        }

        private string GetStringMonth(int month)
        {
            string[] ds = {"","Tháng 1","Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
                "Tháng 7", "Tháng 8","Tháng 9","Tháng 10","Tháng 11","Tháng 12"};
            return ds[month];
        }

        private string GetDateOnButton(Button e)
        {
            string res = "";
            /*
             * 28/2/2023: hello
             * select *from baiKiemTra
             * where month(ngaykiemtra)=@p1 and year(ngaykiemtra)=@p2
             * Mình sẽ truyền @p1 -> giá trị tháng hiện tại, @p2 -> giá trị của năm hiện tại
             * -> Trả về 1 List<Job> trong đó Job chứa DateTime, tên sự kiện, 
             * for cho button, mỗi button đi vào List kiếm , nếu xuất hiện sự kiện thì break
             * hiển thị form và add sự kiện vào cho form hiển thị
             */
            return res;
        }
        #endregion
        //Events
        #region Events
        private void buttonDate_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            MessageBox.Show(btn.Text);
        }
        private void buttonToday_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            MONTH = now.Month;
            YEAR = now.Year;
            lbDate.Text = GetStringMonth(MONTH) + "  " + YEAR;
            LoadDays();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            buttonToday_Click(sender, e);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            MONTH++;
            if (MONTH == 13)
            {
                YEAR++;
                MONTH = 1;
            }
            lbDate.Text = GetStringMonth(MONTH) + "  " + YEAR;
            LoadDays();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            MONTH--;
            if (MONTH == 0)
            {
                YEAR--;
                MONTH = 12;
            }
            lbDate.Text = GetStringMonth(MONTH) + "  " + YEAR;
            LoadDays();
        }
        #endregion
    }
}
