# 🧩 Parte 5 - Encriptado de Datos

## 🎯 Objetivo

Implementar una clase en la capa **Servicios** que permita encriptar cadenas de texto utilizando el algoritmo MD5 provisto por el framework .NET. Esta funcionalidad será utilizada principalmente para encriptar contraseñas de usuarios en el sistema de peluquería.

---

## ⚙️ Requisitos Generales

- Lenguaje: C#
- Framework: .NET 8.0
- UI: Windows Forms
- Arquitectura: N Capas
- Base de datos: SQL Server (`PeluSystem`)
- Principios: 
  - Clean Code
  - Clean Architecture
  - SOLID
  - DRY
  - YAGNI
  - KISS
  - Dependency Injection
  - Expression Body (cuando sea posible)
  - Constructor primario sin campos
  - Funciones con único `return`

---

## 🧱 Capas involucradas y clases requeridas

| Capa     | Clase requerida   | Descripción |
|----------|------------------|-------------|
| `SERV`   | `Encriptar`      | Clase encargada de encriptar cadenas de texto usando MD5 |

---

## 🧩 Clase Encriptar en `SERV`

### Requerimientos

- Debe ser una clase pública ubicada en la capa `SERV`.
- Debe contener un método estático `CreateMD5(string input)` que reciba un string y devuelva su hash MD5 en formato hexadecimal.
- Debe usar **expression body** cuando sea posible.
- Debe tener un único `return`.
- Debe usar **constructor primario sin campos** (aunque en este caso no se requiera estado, se mantiene la práctica).

### Ejemplo de implementación sugerida

```csharp
using System.Security.Cryptography;
using System.Text;

namespace SERV
{
    public class Encriptar()
    {
        public static string CreateMD5(string input)
        {
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
```

---

## ⚠️ Aclaración extra

> Antes de crear la clase `Encriptar`, el agente debe **revisar si ya existe alguna implementación de encriptación en el proyecto**.  
> - Si existe, debe **adaptarla** para cumplir con los principios definidos (expression body, constructor primario sin campos, único return, KISS, etc.).  
> - Si no existe, debe crear la clase desde cero en la capa `SERV`.

---

## 🧠 Pedido a GitHub Copilot

> - Implementar la clase `Encriptar` en la capa `SERV`.
> - Revisar si ya existe una implementación de encriptación en el proyecto y adaptarla si es necesario.
> - Asegurarse de que el método `CreateMD5` cumpla con:
>   - Uso de **expression body** cuando sea posible.
>   - Único `return`.
>   - Código limpio y claro.
> - Mantener la coherencia con la arquitectura de N capas.
> - Respetar los principios de diseño: SOLID, DRY, YAGNI, KISS, Dependency Injection.

---

## 🙋‍♂️ Pedido del Desarrollador

> "Quiero que esta etapa implemente la clase de encriptado en la capa Servicios, respetando la arquitectura y las buenas prácticas. Que el código sea simple, claro y funcional. Que se utilicen expression body, constructor primario sin campos y funciones con único return siempre que sea posible. Si ya existe una implementación de encriptación, que se adapte para cumplir con estos principios."
