using BLL.Repository;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinUI.DTO;

namespace WinUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        BaseRepository<Categories> categoryRepository = new BaseRepository<Categories>();
        BaseRepository<Products> productRepository = new BaseRepository<Products>();
        Products secProduct;
        private void Form1_Load(object sender, EventArgs e)
        {
            cmbKategorilerDoldur();
            dataGridView1Doldur();
            cmbGuncellemeKategorilerDoldur();
            cmbUrunEklemeKategorlerDoldur();
        }

        private void cmbUrunEklemeKategorlerDoldur()
        {
            cmbUrunEklemeKategorler.DataSource = categoryRepository.GetListAll();
            cmbUrunEklemeKategorler.DisplayMember = "CategoryName";
            cmbUrunEklemeKategorler.ValueMember = "CategoryID";

            cmbUrunEklemeKategorler.SelectedIndex = 0;
        }

        private void cmbGuncellemeKategorilerDoldur()
        {
            cmbGuncellemeKategoriler.DataSource = categoryRepository.GetListAll();
            cmbGuncellemeKategoriler.DisplayMember = "CategoryName";
            cmbGuncellemeKategoriler.ValueMember = "CategoryID";
            cmbGuncellemeKategoriler.SelectedIndex = 0;
        }

        void cmbKategorilerDoldur()
        {
            cmbKategoriler.DataSource = categoryRepository.GetListAll();
            cmbKategoriler.DisplayMember = "CategoryName";
            cmbKategoriler.SelectedIndex = 0;
        }
        void dataGridView1Doldur()
        {
            dataGridView1.DataSource= productRepository.Set().Where(x => x.CategoryID == cmbKategoriler.SelectedIndex + 1).Select(x => new ProductDTO
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                UnitPrice = (decimal)x.UnitPrice,
                UnitsInStock = (short)x.UnitsInStock,
                CategoryName = x.Categories.CategoryName
            }).ToList();
        }

        private void cmbKategoriler_SelectedIndexChanged(object sender, EventArgs e)
        {

            dataGridView1Doldur();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            secilenData();
        }

        private void secilenData()
        {
            int secId = (int)dataGridView1.CurrentRow.Cells[0].Value;
            secProduct = productRepository.GetByID(secId);
            txtGuncellemeUrunAd.Text = secProduct.ProductName;
            txtGuncellemeFiyat.Text = secProduct.UnitPrice.ToString();
            nudGuncellemeStok.Value = (decimal)secProduct.UnitsInStock;
            cmbGuncellemeKategoriler.SelectedIndex = secProduct.Categories.CategoryID - 1;
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            secProduct.ProductName = txtGuncellemeUrunAd.Text;
            secProduct.UnitPrice = Convert.ToDecimal(txtGuncellemeFiyat.Text);
            secProduct.UnitsInStock = (short)nudGuncellemeStok.Value;
            secProduct.CategoryID = Convert.ToInt32(cmbGuncellemeKategoriler.SelectedValue);

            productRepository.Update(secProduct);
            dataGridView1Doldur();
            tabControl1.SelectedTab = tabPage1;
        }

        private void guncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            secilenData();
            tabControl1.SelectedTab = tabPage2;
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int secId = (int)dataGridView1.CurrentRow.Cells[0].Value;
            secProduct = productRepository.GetByID(secId);
            productRepository.Delete(secProduct);
            dataGridView1Doldur();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            Products yeniProduct = new Products();
            yeniProduct.ProductName = txtEklemeUrunAd.Text;
            yeniProduct.UnitPrice = Convert.ToDecimal(txtEklemeFiyat.Text);
            yeniProduct.UnitsInStock = (short)nudEklemeStok.Value;
            yeniProduct.CategoryID = Convert.ToInt32(cmbUrunEklemeKategorler.SelectedValue);
            productRepository.Set().Add(yeniProduct);
            try
            {
                productRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Her koşulda buraya girer
                // Db connection sonlandırması için aşırı gereklidir ! ! ! 
            }

            dataGridView1Doldur();
            tabControl1.SelectedTab = tabPage1;
        }
    }
}
