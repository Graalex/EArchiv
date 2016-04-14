using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using Mariupolgaz.EArchiv.Common.Models;
using Microsoft.Win32;

namespace Mariupolgaz.EArchiv
{
	public class ArchivModelView: BaseViewModel
	{
		private XDocument _base;

		public ArchivModelView()
		{
			this.Abonents = new ObservableCollection<Abonent>();
			this.DocumentKinds = new ObservableCollection<string>();
			this.DocumentKinds.Add("Паспорт");
			this.DocumentKinds.Add("ИНН");
			this.DocumentKinds.Add("Право собственности на землю");
			this.DocumentKinds.Add("Право собственности на домостроение");
			this.DocumentKinds.Add("Договор об оказании услуг по газоснабжению");
			this.DocumentKinds.Add("Разрешение на обработку персональных данных");

			//_base = XDocument.Load(new StreamReader("base.dat"));
		}

		private int? _ls;
		public int? LS {
			get { return _ls; }
			set {
				if(_ls != value) {
					_ls = value;
					RaisePropertyChanged(() => LS);
				}
			}
		}

		public ObservableCollection<Abonent> Abonents { get; private set; }

		private Abonent _abonent;
		public Abonent CurrentAbonent {
			get { return _abonent; }
			set {
				if(_abonent != value) {
					_abonent = value;
					RaisePropertyChanged(() => CurrentAbonent);
				}
			}
		}

		public ObservableCollection<string> DocumentKinds { get; private set; }

		private Document _doc;
		public Document CurrentDocument {
			get { return _doc; }
			set {
				if(_doc != value) {
					_doc = value;
					RaisePropertyChanged(() => CurrentDocument);
				}
			}
		}

		private string _kind;
		public string CurrentKind {
			get { return _kind; }
			set {
				if(_kind != value) {
					_kind = value;
					RaisePropertyChanged(() => CurrentKind);
					changeDocument();
				}
			}
		}

		public ICommand FindAbonent {
			get { return new DelegateCommand(onFindAbonent, canFindAbonent); }
		}

		public ICommand AddDocument {
			get { return new DelegateCommand(onAddDocument); }
		}

		private void onFindAbonent()
		{
			if (this.LS != 0) {
				Finder finder = new Finder();
				Abonent ab = finder.FindAbonent((int)this.LS);
				if(ab != null)
					this.Abonents.Add(ab);
			}

		}

		private bool canFindAbonent()
		{
			return true;
		}

		private void onAddDocument() 
		{
			OpenFileDialog dlg = new OpenFileDialog();
			string fname;
			dlg.Multiselect = false;
			dlg.Filter = "Изображения (jpeg)|*.jpg;(tiff)|*.tif;(bmp)|*.bmp";
			bool? rslt = dlg.ShowDialog();
			if (rslt == true) {
				fname = dlg.FileName;
				string tok = "";
				switch(this.CurrentKind) {
					case "Паспорт":
						tok = "PP";
						break;

					case "ИНН":
						tok = "INN";
						break;
        }
				string dst = "img\\" + this.CurrentAbonent.LS + "-" + tok + "-"; //+ this.CurrentKind + "-";
				FileInfo fi = new FileInfo(fname);
				dst += fi.Name;
        File.Copy(fname, dst, true);

				XElement root = XElement.Load("base.dat");
				var ls =
				from el in root.Elements("abonent")
				where (int)el.Attribute("ls") == this.CurrentAbonent.LS
				select el;

				var ell = ls.ToList();

				if(ell.Count != 0) {
					var fl = ell[0];
					var dl =
					from el in fl.Elements("document")
					where (string)el.Attribute("type") == this.CurrentKind
					select el;

					var dll = dl.ToList();
					if(dll.Count != 0)
					{
						var de = dll[0];
						de.Attribute("src").Value = dst;
					}
					else {
						fl.Add(
							new XElement("document", new XAttribute("type", this.CurrentKind), new XAttribute("src", dst))
						);
					}
				}
				else {
					root.Add(
						new XElement("abonent", 
							new XAttribute("ls", this.CurrentAbonent.LS), 
							new XAttribute("name", this.CurrentAbonent.Family), 
							new XAttribute("address", this.CurrentAbonent.Address),
							new XElement("document", 
								new XAttribute("type", this.CurrentKind), 
								new XAttribute("src", dst)
							)
						)
					);
				}
				root.Save("base.dat");
				
      }


		}

		private void changeDocument()
		{
			var root = XElement.Load("base.dat");
			var els =
			from el in root.Elements("abonent")
			where (int)el.Attribute("ls") == CurrentAbonent.LS
			select el;

			var cab = els.ToList();
			if (cab.Count != 0) {
				var dls =
				from dl in cab[0].Elements("document")
				where (string)dl.Attribute("type") == this.CurrentKind
				select dl;

				var dab = dls.ToList();
				if (dab.Count != 0) {
					string src = dab[0].Attribute("src").Value;

					BitmapImage bi = new BitmapImage();
					
					bi.BeginInit();
					FileStream fs = new FileStream(src, FileMode.Open, FileAccess.Read);

					//bi.UriSource = new Uri(src, UriKind.Relative);
					bi.StreamSource = fs;
					bi.EndInit();
					
					//Uri uri = new Uri(src, UriKind.Relative);
 					string fname = (new FileInfo(src)).Name;
					this.CurrentDocument = new Document(fname, bi);
				}
				else {
					this.CurrentDocument = null;
				}
			}
			else {
				this.CurrentDocument = null;
			}
		}
  }
}
