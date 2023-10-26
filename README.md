# Evaluación Academia .NET (Agosto 2023)
El proyecto está desarrollado con Net Core 6

## **Especificación de la Arquitectura**

### **Capa Controller**
Esta capa actuará como el punto de entrada principal a la API. En los controladores, se debe minimizar la cantidad de lógica y utilizarlos principalmente como un intermediario con la capa de servicios.

### **Capa DTOs**
En esta capa, se diseñan y definen los objetos de transferencia de datos que se utilizan para transportar información entre la capa de controladores y la capa de servicios.

### **Capa Helpers**
En esta capa, se encuentran las utilidades y funciones auxiliares que brindan soporte a diversas partes de la aplicación.

### **Capa DataAccess**
En esta sección, estableceremos el DbContext y crearemos los seeds necesarios para poblar nuestras entidades de datos.

### **Capa Entities**
En este nivel de la arquitectura definiremos todas los modelos de la base de datos.

### **Capa Repositories**
En esta capa definiremos las clases correspondientes para realizar el repositorio genérico y la unidad de trabajo
