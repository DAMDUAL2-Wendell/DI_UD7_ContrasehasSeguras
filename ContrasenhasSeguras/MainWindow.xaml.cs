﻿using System;
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
using ControlzEx.Theming;
using MahApps.Metro.Controls;

namespace ContrasenhasSeguras
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Boolean checkBoxMayus = true;
        Boolean checkBoxMinus = true;
        Boolean checkBoxNumeros = true;
        Boolean checkBoxSimboloss = true;

        String textoLongitudContrasenha = "Longitud de la contraseña: ";

        private readonly List<string> baseThemes = new List<string> { "Dark", "Light" };

        private readonly List<string> accentColors = new List<string>
            {
                "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan",
                "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow",
                "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna"
            };


        public MainWindow()
        {
            InitializeComponent();

            // Suscribirse al evento Loaded para esperar hasta que la ventana esté completamente cargada
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Luego de cargar la ventana, se deben cargar los colores de acento correspondientes
            ApplyAccentColors(baseThemeComboBox.SelectedItem as string);
        }




        private void BaseThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Aplicar los colores de acento correspondientes al tema base seleccionado
            ApplyAccentColors(baseThemeComboBox.SelectedItem as string);
        }

        private void ApplyAccentColors(string baseTheme)
        {
            // Limpiar los items anteriores
            accentColorComboBox.Items.Clear();

            foreach (var color in accentColors)
            {
                accentColorComboBox.Items.Add(color);
            }

            // Seleccionar el primer color de acento por defecto
            accentColorComboBox.SelectedIndex = 0;
        }

        private void ApplyTheme_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el tema base y el color de acento seleccionados

            string baseTheme = null;

            // Recorrer los elementos del ComboBox baseThemeComboBox
            foreach (var item in baseThemeComboBox.Items)
            {
                // Verificar si el elemento actual está seleccionado
                if (item is ComboBoxItem comboBoxItem && comboBoxItem.IsSelected)
                {
                    // Obtener el contenido del ComboBoxItem seleccionado
                    baseTheme = comboBoxItem.Content as string;
                    break; // Salir del bucle una vez que se encuentra el elemento seleccionado
                }
            }


            var accentColor = accentColorComboBox.SelectedItem as string;

            MessageBox.Show(baseTheme + ", " + accentColor);

            // Verificar que los valores no sean nulos antes de llamar al método ChangeTheme
            if (baseTheme != null && accentColor != null)
            {
                // Aplicar el tema a la aplicación
                ThemeManager.Current.ChangeTheme(this, $"{baseTheme}.{accentColor}");
            }
            else
            {
                // Manejar el caso en el que alguno de los valores sea nulo
                MessageBox.Show("Seleccione un tema y un color de acento antes de aplicar el tema.");
            }
        }




        private void AsignarCheckBox()
        {
            checkBoxMayus = checkBoxABC.IsChecked.Value;
            checkBoxMinus = checkBoxabc.IsChecked.Value;
            checkBoxNumeros = checkBox123.IsChecked.Value;
            checkBoxSimboloss = checkBoxSimbolos.IsChecked.Value;
        }

        public Boolean AsignarContrasenha(string contrasenha)
        {
            try { textBoxContrasenhaGenerada.Text = contrasenha; return true; } catch (Exception e) { return false; }
        }

        private void ClickCopiar(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textBoxContrasenhaGenerada.Text);
            MessageBox.Show("Contraseña copiada al portapapeles.");
        }
        private void ClickRenovar(object sender, RoutedEventArgs e)
        {
            AsignarCheckBox();

            if(checkBoxNumeros || checkBoxMayus || checkBoxMinus || checkBoxSimboloss)
            {
                // Generar una nueva contraseña
                string nuevaContrasenha = GenerarContrasenha();

                // Asignar la nueva contraseña al TextBox
                AsignarContrasenha(nuevaContrasenha);

                // Ahora que el TextBox tiene la nueva contraseña, actualiza el ProgressBar
                AsignarValorProgressBar();
            }


        }


        private static int CalcularFortalezaContrasenha(string contrasenha)
        {
            int longitud = contrasenha.Length;
            bool contieneMayusculas = false;
            bool contieneMinusculas = false;
            bool contieneNumeros = false;
            bool contieneSimbolos = false;

            foreach (char caracter in contrasenha)
            {
                if (char.IsUpper(caracter)) contieneMayusculas = true;
                else if (char.IsLower(caracter)) contieneMinusculas = true;
                else if (char.IsDigit(caracter)) contieneNumeros = true;
                else contieneSimbolos = true;
            }

            int fortaleza = 0;

            if (longitud >= 8 && longitud < 10) fortaleza += 1;
            else if (longitud >= 10 && longitud < 12) fortaleza += 2;
            else if (longitud >= 12 && longitud < 14) fortaleza += 3;
            else if (longitud >= 14 && longitud < 16) fortaleza += 4;
            else if (longitud >= 16 && longitud < 18) fortaleza += 5;
            else if (longitud >= 18 && longitud < 20) fortaleza += 6;
            else if (longitud >= 20 && longitud < 30) fortaleza += 7;
            else if (longitud >= 30 && longitud < 40) fortaleza += 8;
            else if (longitud >= 40 && longitud < 50) fortaleza += 9;
            else if (longitud >= 50) fortaleza += 10;

            if (contieneMayusculas) fortaleza += 4;
            if (contieneMinusculas) fortaleza += 4;
            if (contieneNumeros) fortaleza += 4;
            if (contieneSimbolos) fortaleza += 4;

            // Asegurar que la fortaleza esté dentro del rango de 0 a 26
            fortaleza = Math.Min(26, Math.Max(0, fortaleza));

            return fortaleza;
        }

        private string GenerarContrasenha()
        {
            var rand = new Random();

            string ret;
            bool requerimientos = false;

            do {
                ret = "";
                for (int x = 0; x < Convert.ToInt32(slider.Value); x++)
                {
                    ret += GenerarUnSoloValorContrasenha(rand);
                }
                requerimientos = comprobarRequerimientosContrasenha(ret);

                // Repetir bucle y generar una nueva contraseña si la anterior no cumple los requisitos,
                // si estan marcados los 4 checkBox la contraseña debe tener al menos un caracter de cada
                // de los marcados es de cir una mayuscula, una minuscula, un numero y un simbolo, a menos
                // que la contraseña tenga longitud 3 en cuyo caso es imposible que se cumpla eso.
            } while (!requerimientos && ret.Length >= 4);

            AsignarValorProgressBar();

            CambiarLabelContrasehaSegura(ObtenerNivelSeguridad(CalcularFortalezaContrasenha(ret)));

            AsignarColorProgressBar(CalcularFortalezaContrasenha(ret));

            return ret;
        }

        private Boolean comprobarRequerimientosContrasenha(String Contrasenha)
        {
            bool contieneMayusculas = false;
            bool contieneMinusculas = false;
            bool contieneNumeros = false;
            bool contieneSimbolos = false;

            foreach (char caracter in Contrasenha)
            {
                if (char.IsUpper(caracter)) contieneMayusculas = true;
                else if (char.IsLower(caracter)) contieneMinusculas = true;
                else if (char.IsDigit(caracter)) contieneNumeros = true;
                else contieneSimbolos = true;
            }

            return (
                contieneMayusculas == checkBoxMayus && 
                contieneMinusculas == checkBoxMinus && 
                contieneNumeros == checkBoxNumeros && 
                contieneSimbolos == checkBoxSimboloss
                );
        }


        private string GenerarUnSoloValorContrasenha(Random rand)
        {
            string ret = "";

            while (ret == "")
            {
                int aleatorio = rand.Next(4);

                switch (aleatorio)
                {
                    case 0:
                        if (checkBoxMayus)
                            ret += GetRandomString(rand, checkBoxMayus);
                        break;
                    case 1:
                        if (checkBoxMinus)
                            ret += GetRandomString(rand, !checkBoxMinus);
                        break;
                    case 2:
                        if (checkBoxNumeros)
                            ret += NumeroAleatorio(rand, 0, 9).ToString();
                        break;
                    case 3:
                        if (checkBoxSimboloss)
                            ret += SimboloAleatorio(rand).ToString();
                        break;
                    default:
                        break;
                }
            }

            return ret;
        }

        private void AsignarValorProgressBar()
        {
            int valor = CalcularFortalezaContrasenha(textBoxContrasenhaGenerada.Text);
            progressBar.Value = valor;
        }

        private string ObtenerNivelSeguridad(int fortaleza)
        {
            string nivelSeguridad;

            switch (fortaleza)
            {
                case int n when (n < 8):
                    nivelSeguridad = "Contraseña demasiado corta o \nno cumple con los requisitos mínimos";
                    break;
                case int n when (n >= 8 && n < 12):
                    nivelSeguridad = "Contraseña débil";
                    break;
                case int n when (n >= 12 && n < 15):
                    nivelSeguridad = "Contraseña algo débil";
                    break;
                case int n when (n >= 15 && n < 17):
                    nivelSeguridad = "Contraseña intermedia";
                    break;
                case int n when (n == 17):
                    nivelSeguridad = "Contraseña segura";
                    break;
                case int n when (n >= 18 && n < 20):
                    nivelSeguridad = "Contraseña bastante segura";
                    break;
                case int n when (n >= 20 && n < 22):
                    nivelSeguridad = "Contraseña muy segura";
                    break;
                case int n when (n >= 22 && n < 24):
                    nivelSeguridad = "Contraseña altamente segura";
                    break;
                case int n when (n >= 24 && n < 26):
                    nivelSeguridad = "Contraseña extremadamente segura";
                    break;
                case int n when (n == 26):
                    nivelSeguridad = "Contraseña prácticamente invulnerable";
                    break;
                default:
                    nivelSeguridad = "Nivel de seguridad desconocido";
                    break;
            }

            return nivelSeguridad;
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

        private void AsignarColorProgressBar(int fortaleza)
        {
            // Define los colores para los diferentes niveles de fortaleza
            Color colorRojo = Colors.Red; // Color para los niveles más bajos de fortaleza
            Color colorAmarillo = Colors.Yellow; // Color intermedio entre rojo y verde
            Color colorVerde = Colors.Green; // Color para fortaleza media
            Color colorAzul = Colors.Blue; // Color para los niveles más altos de fortaleza

            // Interpola los componentes de color entre los diferentes colores en función de la fortaleza
            byte nuevoRojo;
            byte nuevoVerde;
            byte nuevoAzul;

            if (fortaleza <= 10)
            {
                // Interpolación desde rojo a amarillo
                nuevoRojo = InterpolateColorComponent(colorRojo.R, colorAmarillo.R, fortaleza, 10);
                nuevoVerde = InterpolateColorComponent(colorRojo.G, colorAmarillo.G, fortaleza, 10);
                nuevoAzul = InterpolateColorComponent(colorRojo.B, colorAmarillo.B, fortaleza, 10);
            }
            else if (fortaleza <= 16)
            {
                // Interpolación desde amarillo a verde
                nuevoRojo = InterpolateColorComponent(colorAmarillo.R, colorVerde.R, fortaleza - 10, 6);
                nuevoVerde = InterpolateColorComponent(colorAmarillo.G, colorVerde.G, fortaleza - 10, 6);
                nuevoAzul = InterpolateColorComponent(colorAmarillo.B, colorVerde.B, fortaleza - 10, 6);
            }
            else if (fortaleza <= 23)
            {
                // Interpolación desde verde a azul
                nuevoRojo = InterpolateColorComponent(colorVerde.R, colorAzul.R, fortaleza - 16, 7);
                nuevoVerde = InterpolateColorComponent(colorVerde.G, colorAzul.G, fortaleza - 16, 7);
                nuevoAzul = InterpolateColorComponent(colorVerde.B, colorAzul.B, fortaleza - 16, 7);
            }
            else
            {
                // Asignar color azul para fortalezas mayores
                nuevoRojo = colorAzul.R;
                nuevoVerde = colorAzul.G;
                nuevoAzul = colorAzul.B;
            }

            // Crea el nuevo color
            Color nuevoColor = Color.FromRgb(nuevoRojo, nuevoVerde, nuevoAzul);

            // Crea el pincel con el nuevo color
            SolidColorBrush brush = new SolidColorBrush(nuevoColor);

            // Asigna el pincel al ProgressBar
            progressBar.Foreground = brush;
        }

        private byte InterpolateColorComponent(byte inicio, byte fin, int valor, int maximo)
        {
            // Interpola el componente de color entre el inicio y el fin en función del valor
            double porcentaje = (double)valor / maximo;
            double nuevoComponente = inicio + (fin - inicio) * porcentaje;
            return (byte)nuevoComponente;
        }




    }
}
