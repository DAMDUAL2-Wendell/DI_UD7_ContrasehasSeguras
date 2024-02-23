using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContrasenhasSeguras
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Boolean contieneMayus = true;
        Boolean contieneMinus = true;
        Boolean contieneNumeros = true;
        Boolean contieneSimbolos = true;


        String textoLongitudContrasenha = "Longitud de la contraseña: ";

        public MainWindow()
        {
            InitializeComponent();
            AsignarCheckBox();

            AsignarContrasenha(GenerarContrasenha());
        }

        private void AsignarCheckBox()
        {
            contieneMayus = checkBoxABC.IsChecked.Value;
            contieneMinus = checkBoxabc.IsChecked.Value;
            contieneNumeros = checkBox123.IsChecked.Value;
            contieneSimbolos = checkBoxSimbolos.IsChecked.Value;
        }

        public Boolean AsignarContrasenha(string contrasenha)
        {
            try { textBoxContrasenhaGenerada.Text = contrasenha; return true; } catch (Exception e) { return false; }
        }

        private void ClickCopiar(object sender, RoutedEventArgs e)
        {
            string valor = "";
            valor = textBoxContrasenhaGenerada.Text;
        }

        private void ClickRenovar(object sender, RoutedEventArgs e)
        {
            AsignarCheckBox();

            AsignarContrasenha(GenerarContrasenha());
        }

        private String GenerarContrasenha()
        {
            var rand = new Random();

            var resultados = new List<string>();

            for (int x = 0; x <= Convert.ToInt32(slider.Value)-1; x++)
            {
                resultados.Add(GenerarUnSoloValorContrasenha(rand));
            }

            string ret = "";

            resultados.ForEach(x => ret += x.ToString());

            CambiarLabelContrasehaSegura(ValoresContraseha(ComprobarFortalezaContrasenha()));


            AsignarValorProgressBar();

            return ret;
        }

        private string GenerarUnSoloValorContrasenha(Random rand)
        {
            string ret = "";

            int aleatorio = rand.Next(4);

            switch (aleatorio)
            {
                case 0:
                    if (contieneMayus) ret += GetRandomString(rand, contieneMayus);
                    else GenerarUnSoloValorContrasenha(rand);
                    break;
                case 1:
                    if (contieneMinus) ret += GetRandomString(rand, !contieneMinus);
                    else GenerarUnSoloValorContrasenha(rand);
                    break;
                case 2:
                    if (contieneNumeros) ret += NumeroAleatorio(rand, 0, 9).ToString();
                    else GenerarUnSoloValorContrasenha(rand);
                    break;
                case 3:
                    if (contieneSimbolos) ret += SimboloAleatorio(rand).ToString();
                    else GenerarUnSoloValorContrasenha(rand);
                    break;
                default:
                    GenerarUnSoloValorContrasenha(rand);
                    break;
            }
            return ret;
        }

        private void AsignarValorProgressBar()
        {
            int valor = ComprobarFortalezaContrasenha();
            if (valor >= 0 && valor <= 12) progressBar.Value = valor;
        }

        private string ValoresContraseha(int valor)
        {
            String[] valores =
            {
                "Contraseña demasiado corta.",
                "Contraseña débil.",
                "Contraseña media.",
                "Contraseña fuerte.",
                "Contraseña muy fuerte."
            };
            if (valor >= 0 && valor <= valores.Length -1)
            {
                return valores[valor];
            }
            else return valores[valores.Length - 1];
        }

        private int ComprobarFortalezaContrasenha()
        {
            int fortalezaContrasenha = -1;
            if (slider.Value < 8) fortalezaContrasenha = 0;
            if (slider.Value >= 8 && (((contieneMayus) && (contieneMinus)) || (contieneNumeros) || (contieneSimbolos))) fortalezaContrasenha = 1;
            if ((slider.Value >= 8 && (((contieneMayus) && (contieneMinus)) || (contieneNumeros) && (contieneSimbolos)))
                &&
                (slider.Value >= 8 && (((contieneMayus) && (contieneMinus)) && (contieneNumeros) || (contieneSimbolos)))) fortalezaContrasenha = 2;
            if (slider.Value >= 8 && (this.contieneMayus == true) && (this.contieneMinus == true) && (contieneNumeros) && (contieneSimbolos)) fortalezaContrasenha = 3;

            if (slider.Value >= 12 && (((contieneMayus) && (contieneMinus)) || (contieneNumeros) || (contieneSimbolos))) fortalezaContrasenha = 4;
            if ((slider.Value >= 12 && (((contieneMayus) && (contieneMinus)) || (contieneNumeros) && (contieneSimbolos)))
                &&
                (slider.Value >= 12 && (((contieneMayus) && (contieneMinus)) && (contieneNumeros) || (contieneSimbolos)))) fortalezaContrasenha = 5;
            if (slider.Value >= 12 && (this.contieneMayus == true) && (this.contieneMinus == true) && (contieneNumeros) && (contieneSimbolos)) fortalezaContrasenha = 6;

            if (slider.Value >= 15 && (((contieneMayus) && (contieneMinus)) || (contieneNumeros) || (contieneSimbolos))) fortalezaContrasenha = 7;
            if ((slider.Value >= 15 && (((contieneMayus) && (contieneMinus)) || (contieneNumeros) && (contieneSimbolos)))
                &&
                (slider.Value >= 15 && (((contieneMayus) && (contieneMinus)) && (contieneNumeros) || (contieneSimbolos)))) fortalezaContrasenha = 8;
            if (slider.Value >= 15 && (this.contieneMayus == true) && (this.contieneMinus == true) && (contieneNumeros) && (contieneSimbolos)) fortalezaContrasenha = 8;

            if (slider.Value >= 20 && (((contieneMayus) && (contieneMinus)) || (contieneNumeros) || (contieneSimbolos))) fortalezaContrasenha = 9;
            if ((slider.Value >= 20 && (((contieneMayus) && (contieneMinus)) || (contieneNumeros) && (contieneSimbolos)))
                &&
                (slider.Value >= 20 && (((contieneMayus) && (contieneMinus)) && (contieneNumeros) || (contieneSimbolos)))) fortalezaContrasenha = 10;
            if (slider.Value >= 20 && (this.contieneMayus == true) && (this.contieneMinus == true) && (contieneNumeros) && (contieneSimbolos)) fortalezaContrasenha = 11;

            return fortalezaContrasenha;
        }

        private void CambiarLabelContrasehaSegura(String cadena)
        {
            labelEsSegura.Content = cadena;
        }

        private char SimboloAleatorio(Random rand)
        {
            char[] simbolos = ".;@".ToCharArray();
            return simbolos[rand.Next(0, simbolos.Length)];
        }

        private int NumeroAleatorio(Random rand, int min, int max)
        {
            return rand.Next(min, max);
        }

        public static string GetRandomString(Random rand, Boolean mayuscula)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz";
            string ret = new string(chars.Select(c => chars[rand.Next(chars.Length)]).Take(1).ToArray());
            return mayuscula ? ret.ToUpper() : ret;
        }

        private string LetraAleatoria(Random rand, Boolean lowercase)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            char valor = (char)rand.Next(0, 26);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(valor);
            return lowercase ? stringBuilder.ToString().ToLower() : stringBuilder.ToString().ToUpper();
        }

        private void CambiarValorLabelLongitud(string cadena)
        {
            if(labelLongitud != null)
            {
                labelLongitud.Content = this.textoLongitudContrasenha + cadena;
            }
        }


        private void ValueChangedSlider(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CambiarValorLabelLongitud(Convert.ToInt32(slider.Value).ToString());
        }
    }
}
