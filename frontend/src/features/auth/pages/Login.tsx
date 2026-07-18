import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useLogin } from "../hooks/useLogin";
import { getErrorMessage } from "@/core/errors/getErrorMessage";

import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { Button } from "primereact/button";
import { Message } from "primereact/message";
import { Checkbox } from "primereact/checkbox";

const loginStyles = `
  .presto-login-page .presto-login-checkbox .p-checkbox-box,
  .presto-login-page .presto-login-checkbox .p-checkbox-input {
    border-color: #03298e !important;
  }

  .presto-login-page .presto-login-checkbox .p-checkbox-box.p-highlight,
  .presto-login-page .presto-login-checkbox.p-checkbox-checked .p-checkbox-box,
  .presto-login-page .presto-login-checkbox .p-checkbox-box[data-p-highlight="true"],
  .presto-login-page .presto-login-checkbox .p-checkbox-input:checked + .p-checkbox-box {
    background: #03298e !important;
    border-color: #03298e !important;
  }

  .presto-login-page .presto-login-checkbox .p-checkbox-box .p-checkbox-icon,
  .presto-login-page .presto-login-checkbox.p-checkbox-checked .p-checkbox-icon {
    color: #ffffff !important;
  }

  .presto-login-page .presto-login-checkbox .p-checkbox-box:not(.p-disabled):hover {
    border-color: #03298e !important;
  }

  .presto-login-page .presto-login-checkbox .p-checkbox-box.p-focus,
  .presto-login-page .presto-login-checkbox .p-checkbox-input:focus + .p-checkbox-box {
    box-shadow: 0 0 0 0.2rem rgba(3, 41, 142, 0.14) !important;
    border-color: #03298e !important;
  }

  .presto-login-page .presto-login-password,
  .presto-login-page .presto-login-password .p-password,
  .presto-login-page .presto-login-password .p-icon-field,
  .presto-login-page .presto-login-password .p-inputtext,
  .presto-login-page .presto-login-password .p-password-input {
    width: 100% !important;
  }

  .presto-login-page .presto-login-password {
    display: block !important;
  }

  .presto-login-page .presto-login-password .p-inputtext {
    height: 52px !important;
    border-radius: 0.75rem !important;
    border: 1px solid #ebebeb !important;
    background: #fafafa !important;
    padding-left: 3rem !important;
    padding-right: 3rem !important;
    font-size: 15px !important;
    color: #333333 !important;
    box-shadow: none !important;
  }

  .presto-login-page .presto-login-submit {
    width: 100% !important;
    min-width: 100% !important;
    max-width: 100% !important;
    display: flex !important;
    align-items: center !important;
    justify-content: center !important;
  }

  .presto-login-page .presto-login-submit .p-button-label {
    flex: 0 0 auto !important;
    width: auto !important;
  }
`;

function PrestoLogoMark({ className = "" }: { className?: string }) {
  const onSamlLogin = () => {
    window.location.assign(`${apiBaseUrl}/api/security/saml/login`);
  };

  return (
    <div
      className={`relative flex shrink-0 items-center justify-center rounded-[14px] bg-[#03298e] font-bold text-white ${className}`}
    >
      <span className="relative leading-none">
        P
        <sup className="absolute -right-[9px] -top-[2px] text-[14px] font-bold leading-none">°</sup>
      </span>
    </div>
  );
}

export default function Login() {
  const useSaml = import.meta.env.VITE_AUTH_USE_SAML === "true";
  const apiBaseUrl = String(import.meta.env.VITE_API_BASE_URL ?? "").replace(/\/$/, "");

  const navigate = useNavigate();
  const loginMutation = useLogin();

  const [user_name, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [rememberSession, setRememberSession] = useState(true);
  const [error, setError] = useState("");

  const onSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError("");

    try {
      const response = await loginMutation.mutateAsync({
        user_name,
        password,
      });

      if (response.status && response.detail?.token_multibanca) {
        navigate("/home/bandeja");
      } else {
        setError(response.message || "No se pudo iniciar sesión");
      }
    } catch (error) {
      setError(getErrorMessage(error));
    }
  };

  return (
    <main className="presto-login-page min-h-screen w-full bg-white lg:flex">
      <style>{loginStyles}</style>
      {/* LADO IZQUIERDO - DESKTOP */}
      <section className="relative hidden min-h-screen w-1/2 overflow-hidden bg-[#010a33] px-14 py-12 lg:flex lg:flex-col lg:justify-between">
        <div className="absolute -left-32 -top-32 h-[500px] w-[500px] rounded-full bg-[#03298e] opacity-[0.12]" />
        <div className="absolute -bottom-28 -right-28 h-[420px] w-[420px] rounded-full bg-[#03298e] opacity-[0.08]" />

        <div className="relative z-10 flex items-center gap-4">
          <PrestoLogoMark className="h-[52px] w-[52px] text-xl" />
          <div>
            <div className="text-base font-semibold text-white">Presto Multibanca</div>
          </div>
        </div>

        <div className="relative z-10">
          <h1 className="max-w-md font-serif text-[52px] leading-[1.08] text-white">
            Formalización
            <br />
            Hipotecaria
          </h1>
        </div>

        <div className="relative z-10">
          <div className="inline-flex items-center gap-2 rounded-[10px] border border-white/10 bg-white/5 px-4 py-3 text-xs text-white/45">
            <i className="pi pi-shield text-[#03298e]" />
            <span>Conexión cifrada</span>
          </div>
        </div>
      </section>

      {/* FORMULARIO */}
      <section className="flex min-h-screen flex-1 items-center justify-center bg-white px-6 py-10 sm:px-10 lg:px-20">
        <div className="w-full max-w-[420px]">
          {/* LOGO MOBILE */}
          <div className="mb-8 flex justify-center lg:hidden">
            <PrestoLogoMark className="h-[54px] w-[54px] text-xl" />
          </div>

          <div className="mb-9">
            <h2 className="mb-2 text-[34px] font-semibold tracking-[0.08em] text-[#111] sm:text-[36px]">
              Inicia sesión
            </h2>
            <p className="text-sm text-[#888]">
              {useSaml
                ? "Ingresa con tu identidad corporativa BBVA"
                : "Ingresa con tus Credenciales"}
            </p>
          </div>

          {error && (
            <div className="mb-5">
              <Message severity="error" text={error} />
            </div>
          )}

          {useSaml ? (
            <Button
              type="button"
              onClick={onSamlLogin}
              style={{ width: "100%", minWidth: "100%", maxWidth: "100%" }}
              className="presto-login-submit mx-auto flex h-[56px] !w-full min-w-full items-center justify-center rounded-xl border-none bg-[#03298e] text-[15px] font-semibold text-white transition-colors hover:bg-[#021f6b]"
            >
              <span>Iniciar sesión con BBVA</span>
              <i className="pi pi-arrow-right ml-2 text-sm" />
            </Button>
          ) : (
            <form onSubmit={onSubmit} className="space-y-5">
              <div>
                <label
                  htmlFor="user_name"
                  className="mb-2 block text-[10px] font-semibold uppercase tracking-[0.18em] text-[#b5b5b5]"
                >
                  Usuario
                </label>

                <span className="relative block w-full">
                  <i className="pi pi-envelope pointer-events-none absolute left-4 top-1/2 z-10 -translate-y-1/2 text-[#c8c8c8]" />
                  <InputText
                    id="user_name"
                    name="user_name"
                    autoComplete="username"
                    value={user_name}
                    onChange={(e) => setUserName(e.target.value)}
                    placeholder="correo@empresa.com"
                    className="h-[52px] w-full rounded-xl border border-[#ebebeb] bg-[#fafafa] pl-12 pr-4 text-[15px] text-[#333] shadow-none"
                  />
                </span>
              </div>

              <div>
                <label
                  htmlFor="password"
                  className="mb-2 block text-[10px] font-semibold uppercase tracking-[0.18em] text-[#b5b5b5]"
                >
                  Contraseña
                </label>

                <span className="relative block w-full">
                  <i className="pi pi-lock pointer-events-none absolute left-4 top-1/2 z-10 -translate-y-1/2 text-[#c8c8c8]" />
                  <Password
                    inputId="password"
                    name="password"
                    autoComplete="current-password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    feedback={false}
                    toggleMask
                    placeholder="••••••••••"
                    inputClassName="h-[52px] w-full rounded-xl border border-[#ebebeb] bg-[#fafafa] pl-12 pr-12 text-[15px] text-[#333] shadow-none"
                    className="presto-login-password w-full"
                    inputStyle={{ width: "100%" }}
                    style={{ width: "100%", display: "block" }}
                  />
                </span>
              </div>

              <div className="flex items-center justify-between gap-4">
                <label htmlFor="rememberSession" className="flex cursor-pointer items-center gap-2 text-[13px] text-[#888]">
                  <Checkbox
                    inputId="rememberSession"
                    checked={rememberSession}
                    onChange={(e) => setRememberSession(Boolean(e.checked))}
                    className="presto-login-checkbox [&_.p-checkbox-box]:border-[#03298e] [&_.p-checkbox-box.p-highlight]:bg-[#03298e] [&_.p-checkbox-box.p-highlight]:border-[#03298e]"
                  />
                  <span>Mantener sesión</span>
                </label>

                <button
                  type="button"
                  className="text-[13px] font-medium text-[#03298e] hover:underline"
                >
                  ¿Olvidaste tu contraseña?
                </button>
              </div>

              <Button
                type="submit"
                loading={loginMutation.isPending}
                disabled={loginMutation.isPending}
                style={{ width: "100%", minWidth: "100%", maxWidth: "100%" }}
                className="presto-login-submit mx-auto flex h-[56px] !w-full min-w-full items-center justify-center rounded-xl border-none bg-[#03298e] text-[15px] font-semibold text-white transition-colors hover:bg-[#021f6b]"
              >
                <span>
                  {loginMutation.isPending ? (
                    "Ingresando..."
                  ) : (
                    <>
                      Ingresar a PRESTO<span className="relative -top-[0.35em] ml-[1px] text-[0.62em] leading-none">®</span>
                    </>
                  )}
                </span>
                {!loginMutation.isPending && <i className="pi pi-arrow-right ml-2 text-sm" />}
              </Button>
            </form>
          )}

          <div className="mt-6 text-center text-[10px] uppercase tracking-[0.2em] text-[#d0d0d0]">
            Powered by <span className="font-semibold text-[#aaa]">Cibergestión</span>
          </div>
        </div>
      </section>
    </main>
  );
}
