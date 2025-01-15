PROYECTO PRUEBA TECNICA API

DESCRIPCIÓN DE USO DE API .NETCORE

AUTENTICACIÓN
Para autenticarte, realiza una solicitud POST a la ruta /api/auth/login con los siguientes datos en el cuerpo de la solicitud:

SCRIPT CREACIÓN DE PRODUCTOS

{
  "user": "admin"
  "password" : "admin123!"
}


INSERT INTO Productos (Nombre, Precio, Stock, FechaCreacion)
VALUES
    ('Producto 1', 100.00, 10, '2025-01-15T14:30:00'),
    ('Producto 2', 200.00, 20,'2025-01-15T14:30:00'),
    ('Producto 3', 150.00, 15, '2025-01-15T14:30:00'),
    ('Producto 4', 250.00, 25, '2025-01-15T14:30:00'),
    ('Producto 5', 300.00, 30, '2025-01-15T14:30:00');
		
