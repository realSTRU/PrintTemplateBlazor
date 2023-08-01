using System;
using System.Drawing.Printing;
using System.Drawing;
using System.Text;
using System.IO;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

public class FacturaPrinter
{
    public static void ImprimirFactura()
    {
        string factura = GenerarContenidoFactura(); // Implementa la lógica para generar el contenido de la factura

        // Conectarse a la impresora a través de Bluetooth
        BluetoothDeviceInfo printerDevice = FindBluetoothPrinter("MTP-2"); // Reemplaza "NombreDeTuImpresora" con el nombre real de tu impresora
        if (printerDevice != null)
        {
            using BluetoothClient bluetoothClient = new BluetoothClient();
            bluetoothClient.Connect(printerDevice.DeviceAddress, BluetoothService.SerialPort);
            if (bluetoothClient.Connected)
            {
                SendDataToBluetoothPrinter(bluetoothClient.GetStream(), factura);
            }
        }
    }

    private static BluetoothDeviceInfo FindBluetoothPrinter(string printerName)
    {
        BluetoothClient bluetoothClient = new BluetoothClient();
        BluetoothDeviceInfo[] devices = bluetoothClient.DiscoverDevices();

        foreach (BluetoothDeviceInfo device in devices)
        {
            if (device.DeviceName == printerName)
            {
                return device;
            }
        }

        return null;
    }

    private static void SendDataToBluetoothPrinter(Stream stream, string content)
    {
        // Convertir el contenido de la factura en bytes para enviar a la impresora
        byte[] data = Encoding.ASCII.GetBytes(content);

        // Enviar los datos a través de Bluetooth
        stream.Write(data, 0, data.Length);
    }

    private static string GenerarContenidoFactura()
    {
        // Implementa la lógica para generar el contenido de la factura como una cadena de texto
        // Puedes utilizar un StringBuilder para facilitar la construcción del contenido
        // Por ejemplo:
        StringBuilder contenido = new StringBuilder();
        contenido.AppendLine("=============================");
        contenido.AppendLine("        Factura");
        contenido.AppendLine("=============================");
        contenido.AppendLine("Cliente: Nombre del cliente aquí");
        contenido.AppendLine("Dirección: Dirección del cliente aquí");
        contenido.AppendLine("Fecha: Fecha de la factura aquí");
        contenido.AppendLine("Número de factura: Número de factura aquí");
        contenido.AppendLine("=============================");
        contenido.AppendLine("Descripción   Cantidad   Precio Unitario   Total");
        contenido.AppendLine("Producto 1    2          $10.00            $20.00");
        contenido.AppendLine("Producto 2    3          $8.00             $24.00");
        contenido.AppendLine("=============================");
        contenido.AppendLine("Total:                   $44.00");
        contenido.AppendLine("=============================");
        contenido.AppendLine("Gracias por su compra!");
        return contenido.ToString();
    }
}
