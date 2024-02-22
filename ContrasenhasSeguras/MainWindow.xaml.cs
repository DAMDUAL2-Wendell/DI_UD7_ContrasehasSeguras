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
        Boolean mayusculas = true;
        Boolean minusculas = true;
        Boolean numeros = true;
        Boolean simbolos = true;
        string textoLongitudContrasenha = "Longitud de la contraseña:";

        public MainWindow()
        {
            InitializeComponent();
            AsignarCheckBox();

            AsignarContrasenha(GenerarContrasenha(mayusculas, minusculas, numeros, simbolos));
        }

        private void AsignarCheckBox()
        {
            mayusculas = checkBoxABC.IsChecked.Value;
            minusculas = checkBoxabc.IsChecked.Value;
            numeros = checkBox123.IsChecked.Value;
            simbolos = checkBoxSimbolos.IsChecked.Value;
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
            AsignarContrasenha(GenerarContrasenha(mayusculas, minusculas, numeros, simbolos));
        }

        private String GenerarContrasenha(Boolean mayusculas, Boolean minusculas, Boolean numeros, Boolean simbolos)
        {
            var rand = new Random();
            var resultados = new List<string>();

            string mayus = "";
            string minus = "";
            int numero = 0;
            char simboloAleatorio = 'A';

            for (int x = 0; x <= Convert.ToInt32(valorSlider); x++)
            {
                if (mayusculas)
                {
                    mayus = GetRandomString(rand,mayusculas);
                    resultados.Add(mayus);
                }
                if (minusculas)
                {
                    minus = GetRandomString(rand,minusculas);
                    resultados.Add(minus);
                }
                if (numeros)
                {
                    numero = NumeroAleatorio(rand, 0, 9);
                    resultados.Add(numero.ToString());
                }
                if (simbolos)
                {
                    simboloAleatorio = SimboloAleatorio(rand);
                    resultados.Add(simboloAleatorio.ToString());
                }
                
            }
            string ret = "";
            resultados.ForEach(x => ret += x.ToString());
            return ret;
        }

        private char SimboloAleatorio(Random rand)
        {
            char[] simbolos = ".;@".ToCharArray();
            return simbolos[rand.Next(0,simbolos.Length)];
        }

        private int NumeroAleatorio(Random rand, int min, int max)
        {
            return rand.Next(min, max);
        }

        public static string GetRandomString(Random rand, Boolean mayuscula)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz";
            string ret = new string(chars.Select(c => chars[rand.Next(chars.Length)]).Take(1).ToArray());
            return mayuscula ? ret.ToUpper() : ret.ToString().ToLower();
        }

        private string LetraAleatoria(Random rand,Boolean lowercase)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            char valor = (char)rand.Next(0, 26);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(valor);
            return lowercase? stringBuilder.ToString().ToLower() : stringBuilder.ToString().ToUpper();
        }

        private void ValueChangedSlider(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelLongitud.Content = textoLongitudContrasenha + slider.Value.ToString();
        }
    }
}
