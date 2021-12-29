using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultithreadAsyncForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public int sayac { get; set; } = 0;
        Stopwatch sp = new Stopwatch();
        private void btnBitir_Click(object sender, EventArgs e)
        {

        }

        private async void btnBaslat_Click(object sender, EventArgs e)
        {
            var aTask = Go(progressBar1);
            var bTask = Go(progressBar2);

            await Task.WhenAll(aTask, bTask);
        }
        public async Task Go(ProgressBar pb)
        {
            sp.Start();
            await Task.Run(() =>
         {
             Enumerable.Range(1, 100).ToList().ForEach(x =>
             {
                 Thread.Sleep(100);

                 pb.Invoke((MethodInvoker)delegate { pb.Value = x; });
                 if (pb.Value == 100)
                 {
                     label1.Invoke((MethodInvoker)delegate
                     {
                         label1.Text += ($"{pb.Name} Tamamlandı. Geçen süre: {sp.Elapsed.TotalSeconds}\n");
                     });
                     sp.Stop();
                 }
             });

                //Enumerable.Range(1, 100).ToList().ForEach(x =>
                // pb.Value = x); Senkron olsaydı olurdu. fakat UI thread kullanımda olduğu için bunu ordan temin edemiyoruz.
            });
        }

        private void btnSayac_Click(object sender, EventArgs e)
        {
            btnSayac.Text = sayac++.ToString();
        }
    }
}
