using Spectre.Console;

// Solicitar los datos del préstamo
decimal monto = AnsiConsole.Ask<decimal>("Ingrese el monto del préstamo:");
decimal tasaAnual = AnsiConsole.Ask<decimal>("Ingrese la tasa de interés anual (%):");
int plazo = AnsiConsole.Ask<int>("Ingrese el plazo del préstamo en meses:");

// Calcular la tasa de interés mensual
decimal tasaMensual = tasaAnual / 100m / 12m;

// Calcular la cuota fija mensual
decimal cuota;

if (tasaMensual == 0)
{
    cuota = monto / plazo;
}
else
{
    double m = (double)monto;
    double i = (double)tasaMensual;

    cuota = (decimal)(
        m * i / (1 - Math.Pow(1 + i, -plazo))
    );
}

cuota = Math.Round(cuota, 2);

// Crear la tabla de amortización
var tabla = new Table();

tabla.Title("TABLA DE AMORTIZACIÓN");
tabla.AddColumn("Cuota");
tabla.AddColumn("Pago mensual");
tabla.AddColumn("Interés");
tabla.AddColumn("Abono a capital");
tabla.AddColumn("Saldo pendiente");

decimal saldo = monto;

for (int mes = 1; mes <= plazo; mes++)
{
    decimal interes = Math.Round(saldo * tasaMensual, 2);
    decimal abonoCapital = Math.Round(cuota - interes, 2);
    decimal pago = cuota;

    // Ajustar la última cuota para saldar la deuda
    if (mes == plazo)
    {
        abonoCapital = saldo;
        pago = Math.Round(abonoCapital + interes, 2);
    }

    saldo = Math.Round(saldo - abonoCapital, 2);

    if (saldo < 0)
    {
        saldo = 0;
    }

    tabla.AddRow(
        mes.ToString(),
        pago.ToString("N2"),
        interes.ToString("N2"),
        abonoCapital.ToString("N2"),
        saldo.ToString("N2")
    );
}

// Mostrar resultados en consola
AnsiConsole.MarkupLine("\n[green]Cuota fija mensual:[/] " + cuota.ToString("N2"));
AnsiConsole.Write(tabla); 
