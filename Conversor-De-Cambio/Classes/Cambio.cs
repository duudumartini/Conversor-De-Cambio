﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Conversor_De_Cambio.Classes;

public class Cambio
{
    public string? Valor {get; set;}

    public DateTime Date {get; set;}

    public string? MoedaBase {get; set;}

    public string? MoedaAlvo {get; set;}

    public decimal Resultado {get; set;}
}
