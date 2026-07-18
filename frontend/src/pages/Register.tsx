import { useState } from "react";
import { Link } from "react-router-dom";
import * as authService from "../features/auth/api/registerService";

// PrimeReact
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { Button } from "primereact/button";
import { Message } from "primereact/message";

export default function Register() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setSuccess("");
    setLoading(true);

    try {
      const data = await authService.registerService(
        username,
        password,
        confirmPassword
      );
      setSuccess(data.message);
    } catch (err: any) {
      setError(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gradient-to-br from-gray-900 via-gray-800 to-gray-950 px-4">
      <div className="w-full max-w-md">
        <Card className="bg-white/90 backdrop-blur-md border border-gray-200 rounded-3xl shadow-2xl p-8 space-y-6 transform transition duration-300 hover:scale-105 hover:shadow-3xl">

          {/* Logo */}
          <div className="flex flex-col items-center space-y-2">
            <div className="w-20 h-20 bg-gradient-to-br from-blue-600 to-purple-600 rounded-3xl flex items-center justify-center text-white text-4xl font-bold shadow-lg">
              M
            </div>
            <h2 className="text-2xl font-bold text-gray-900">Multibanca</h2>
            <p className="text-sm text-gray-600 text-center">
              Crea tu cuenta para continuar
            </p>
          </div>

          {/* Error */}
          {error && <Message severity="error" text={error} />}

          {/* Success */}
          {success && <Message severity="success" text={success} />}

          <form onSubmit={handleRegister} className="flex flex-col space-y-5">

            {/* Usuario */}
            <div className="flex flex-col space-y-1">
              <label className="text-sm text-gray-700">Usuario</label>
              <InputText
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="Ingresa tu usuario"
                className="w-full bg-white/70 border border-gray-300 rounded-xl p-4 backdrop-blur-sm focus:border-blue-600 focus:ring-1 focus:ring-blue-600 shadow-sm transition"
              />
            </div>

            {/* Contraseña */}
            <div className="flex flex-col space-y-1">
              <label className="text-sm text-gray-700">Contraseña</label>
              <Password
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Ingresa tu contraseña"
                toggleMask
                feedback={false}
                className="w-full"
                inputClassName="w-full bg-white/70 border border-gray-300 rounded-xl p-4 backdrop-blur-sm focus:border-blue-600 focus:ring-1 focus:ring-blue-600 shadow-sm transition"
              />
            </div>

            {/* Confirmar Contraseña */}
            <div className="flex flex-col space-y-1">
              <label className="text-sm text-gray-700">Confirmar Contraseña</label>
              <Password
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                placeholder="Confirma tu contraseña"
                toggleMask
                feedback={false}
                className="w-full"
                inputClassName="w-full bg-white/70 border border-gray-300 rounded-xl p-4 backdrop-blur-sm focus:border-blue-600 focus:ring-1 focus:ring-blue-600 shadow-sm transition"
              />
            </div>

            {/* Botón */}
            <Button
              type="submit"
              label={loading ? "Registrando..." : "Registrar"}
              icon="pi pi-user-plus"
              loading={loading}
              className="w-full bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-700 hover:to-purple-700 text-white rounded-xl p-4 font-semibold shadow-lg transition duration-200"
            />

            {/* Login */}
            <p className="text-sm text-center text-gray-600">
              ¿Ya tienes cuenta?{" "}
              <Link
                to="/"
                className="text-blue-600 font-semibold hover:underline"
              >
                Iniciar sesión
              </Link>
            </p>
          </form>
        </Card>

        {/* Footer */}
        <p className="text-center text-xs text-gray-400 mt-8">
          © {new Date().getFullYear()} Multibanca - Todos los derechos reservados
        </p>
      </div>
    </div>
  );
}