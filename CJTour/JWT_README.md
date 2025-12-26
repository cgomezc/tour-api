# JWT Authentication - CJTour API

Este documento describe cómo usar la autenticación JWT implementada en la API de CJTour.

## Configuración

La configuración JWT se encuentra en `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "jEkxWoWix8skZIGE5orfM0I/C0PKffAlJkeUEfmx6NnXqjNn33Ma+JlwJXpSuOk5CuL2oXCumgI2G3FR2pQe1w==",
    "Issuer": "https://localhost:7000",
    "Audience": "https://localhost:7000",
    "ExpirationInMinutes": 60,
    "RefreshTokenExpirationInDays": 7
  }
}
```

## Endpoints de Autenticación

### 1. Registro de Usuario
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "usuario@ejemplo.com",
  "password": "MiPassword123!",
  "firstName": "Juan",
  "lastName": "Pérez",
  "phoneNumber": "+1234567890",
  "dateOfBirth": "1990-01-01"
}
```

**Respuesta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64-encoded-refresh-token",
  "expiresAt": "2024-01-01T10:00:00Z",
  "user": {
    "userId": 1,
    "email": "usuario@ejemplo.com",
    "firstName": "Juan",
    "lastName": "Pérez",
    "phoneNumber": "+1234567890",
    "dateOfBirth": "1990-01-01",
    "createdAt": "2024-01-01T09:00:00Z"
  }
}
```

### 2. Inicio de Sesión
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@ejemplo.com",
  "password": "MiPassword123!"
}
```

### 3. Obtener Usuario Actual
```http
GET /api/auth/me
Authorization: Bearer YOUR_JWT_TOKEN_HERE
```

### 4. Validar Token
```http
GET /api/auth/validate
Authorization: Bearer YOUR_JWT_TOKEN_HERE
```

## Endpoints Protegidos

Los siguientes endpoints requieren autenticación:

- `DELETE /api/tours/{id}` - Eliminar tour
- `GET /api/tours/my-tours` - Obtener tours del usuario
- `GET /api/auth/me` - Información del usuario actual
- `GET /api/auth/validate` - Validar token

## Uso del Token

### En Headers HTTP
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### En JavaScript/TypeScript
```javascript
const token = 'your-jwt-token';

fetch('/api/tours/my-tours', {
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }
})
.then(response => response.json())
.then(data => console.log(data));
```

### En C# (HttpClient)
```csharp
httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", token);
```

## Swagger UI

La API incluye configuración de Swagger con soporte para JWT:

1. Navega a `https://localhost:7053/swagger`
2. Haz clic en el botón "Authorize" 
3. Ingresa `Bearer YOUR_TOKEN_HERE` en el campo de autorización
4. Ahora puedes probar endpoints protegidos directamente desde Swagger

## Manejo de Errores

### 401 Unauthorized
- Token ausente, inválido o expirado
- Credenciales incorrectas en login

### 403 Forbidden
- Token válido pero sin permisos suficientes

### 409 Conflict
- Email ya registrado durante el registro

## Estructura de Claims

El JWT contiene los siguientes claims:

- `nameid` (NameIdentifier): ID del usuario
- `email`: Email del usuario  
- `given_name`: Nombre del usuario
- `family_name`: Apellido del usuario
- `jti`: ID único del token
- `iat`: Timestamp de emisión
- `exp`: Timestamp de expiración
- `iss`: Emisor del token
- `aud`: Audiencia del token

## Configuración de CORS

El CORS está configurado para permitir:
- Orígenes: `https://localhost:3000`, `http://localhost:3000`, `https://localhost:7053`
- Headers: Cualquier header
- Métodos: Cualquier método
- Credenciales: Habilitadas

## Archivos de Prueba

Utiliza `JwtTests.http` para probar todos los endpoints de autenticación con VS Code REST Client.

## Seguridad

- Las contraseñas se almacenan hasheadas con SHA256
- Los tokens expiran en 60 minutos por defecto
- Los refresh tokens expiran en 7 días por defecto
- No hay tolerancia de reloj (ClockSkew = 0)

## Próximas Mejoras

- [ ] Implementar almacenamiento de refresh tokens en base de datos
- [ ] Agregar roles y permisos
- [ ] Implementar logout con invalidación de tokens
- [ ] Agregar rate limiting en endpoints de auth
- [ ] Implementar bloqueo de cuenta por intentos fallidos