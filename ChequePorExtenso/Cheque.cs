using System;
using System.Collections.Generic;

namespace ChequePorExtenso
{
    public class Cheque
    {
        String[] unidades = new String[] { "", "um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove" };
        String[] dezVinte = new String[] { "", "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove" };
        String[] dezenas = new String[] { "", "dez", "vinte", "trinta", "quarenta", "cinquenta", "sessenta", "setenta", "oitenta", "noventa" };
        String[] centenas = new String[] { "", "cento", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos" };
        String[] multiplicadorSingular = new String[] { "centavo", "real", "mil", "milhão", "bilhão", "trilhão", "quadrilhão", "quintilhão", "sextilhão", "septilhão" };
        String[] multiplicadorPlural = new String[] { "centavos", "reais", "mil", "milhões", "bilhões", "trilhões", "quadrilhões", "quintilhões", "sextilhões", "septilhões" };

        private double valor;

        public Cheque(double valor)
        {
            this.valor = valor;
        }

        public string chequePorExtenso(double valor) { this.valor = valor; return chequePorExtenso(); }
        public string chequePorExtenso()
        {
            double max = 989999999999999999999999999d;

            if (valor > max || valor < 0)
                return "Valor não suportado";

            List<String> classes = listaDeClasses();
            Stack<String> chequePorExtenso = new Stack<String>();

            int iMultip = 0;
            bool redondo = EhvalorRedondo(classes);
            for (int i = classes.Count; i > 0; i--)
            {
                String multiplicador = "";
                String classeAtual = classes[i - 1];

                if (classeAtual != "000" || (iMultip == 1 && valor > 1))                                                   //permite colocar "reais" em todas os cheques que possuem valor maior ou igual 1 real
                {
                    if (classeAtual == "001") multiplicador += multiplicadorSingular[iMultip];
                    else multiplicador += multiplicadorPlural[iMultip];

                    if (iMultip == 1 && classes[i] != "000") chequePorExtenso.Push(" e ");                                                         //e para separar centavos
                    else if (iMultip > 1 && classes[i] != "000") chequePorExtenso.Push(" ");
                    else if (i < classes.Count - 2 && redondo) multiplicador += " de";                                                       //inserir "de reais" em valores redondos a partir de 1.000.000
                    else if (i < classes.Count - 2 && !redondo) multiplicador += " e ";
                    chequePorExtenso.Push(multiplicador);
                    chequePorExtenso.Push(" ");
                }
                iMultip++;
                chequePorExtenso.Push(classeEmExtenso(classes[i - 1]));
            }
            return montarChequePorExtenso(chequePorExtenso);
        }
        private bool EhvalorRedondo(List<string> classes)
        {
            int ocorrencias = 0;
            for (int i = classes.Count - 1; i > 0; i--)         //-1 para desconsiderar os centavos
            {
                if (classes[i - 1] != "000") ocorrencias++;
            }
            return ocorrencias == 1;
        }
        private string montarChequePorExtenso(Stack<String> classesPorExtenso)
        {
            String chequePorExtenso = null;
            while (classesPorExtenso.Count > 0)
                chequePorExtenso += classesPorExtenso.Pop();
            return chequePorExtenso;
        }
        private string classeEmExtenso(String valor)
        {
            String separacaoCentenas = "";
            String separacaoDezenas = "";

            int centena = (int)Char.GetNumericValue(valor[0]);
            int dezena = (int)Char.GetNumericValue(valor[1]);
            int unidade = (int)Char.GetNumericValue(valor[2]);

            String centenas = this.centenas[centena];
            String dezenas = this.dezenas[dezena];
            String unidades = this.unidades[unidade];

            if (centena == 1 && dezena == 0 && unidade == 0) centenas = "cem";                                      //100
            else if (centena > 0 && (dezena != 0 || unidade != 0)) separacaoCentenas = " e ";                       //maior que 100 com dezena ou unidade
            if (dezena == 1 && unidade != 0) { dezenas = dezVinte[unidade]; unidades = ""; }                        //11-19
            else if (dezena > 1 && unidade != 0) separacaoDezenas = " e ";                                          //maior que 20 com unidade

            return centenas + separacaoCentenas + dezenas + separacaoDezenas + unidades;
        }
        private List<String> listaDeClasses()                                                                        //centavos por ultimo
        {
            String valorStr = Convert.ToDecimal(valor).ToString("000,000,000,000,000,000,000,000,000.00");
            String[] classesEDecimal = valorStr.Split(',');
            String[] classes = classesEDecimal[0].Split('.');

            List<String> listaClasses = new List<String>();

            foreach (String item in classes)
            {
                listaClasses.Add(item);
            }
            listaClasses.Add("0" + classesEDecimal[1]);

            return listaClasses;
        }
    }
}