import { useNavigate } from "react-router-dom";
import { useAuth } from "@/app/providers/AuthProvider";

export default function NotFound() {
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();

  const handleGoHome = () => {
    navigate(isAuthenticated ? "/home/bandeja" : "/login", { replace: true });
  };

  return (
    <div className="flex min-h-[85vh] w-full items-center justify-center p-4">
      <div className="relative w-full max-w-2xl overflow-hidden rounded-[28px] bg-gradient-to-br from-[#0c1f38] via-[#1e3a5f] to-[#2b59ff] px-6 py-16 text-center shadow-[0_30px_80px_-20px_rgba(15,23,42,0.7)] sm:px-12 sm:py-20">
        <div className="animate-nf-glow pointer-events-none absolute -left-24 -top-24 h-80 w-80 rounded-full bg-[#03298e] blur-3xl" />
        <div className="animate-nf-glow pointer-events-none absolute -bottom-28 -right-24 h-80 w-80 rounded-full bg-[#4a74ff] blur-3xl" />
        <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(circle_at_top,rgba(255,255,255,0.08),transparent_55%)]" />

        <div className="animate-nf-fade-in-up relative flex flex-col items-center">
          <div className="animate-nf-floaty">
            <span className="block bg-gradient-to-b from-white via-[#cfe0ff] to-[#7fa9ff] bg-clip-text text-[110px] font-black leading-none tracking-tighter text-transparent drop-shadow-[0_10px_40px_rgba(74,116,255,0.55)] sm:text-[170px]">
              404
            </span>
          </div>

          <span className="-mt-3 inline-flex items-center gap-2 rounded-full border border-white/20 bg-white/10 px-4 py-1.5 text-xs font-medium uppercase tracking-widest text-blue-100 backdrop-blur-sm">
            <i className="pi pi-compass text-sm" />
            Error 404
          </span>

          <h1 className="mt-7 text-2xl font-semibold text-white sm:text-3xl">
            Página no encontrada
          </h1>

          <p className="mt-3 max-w-md text-sm leading-relaxed text-blue-100/80">
            La página que buscás no existe, fue movida o el enlace que seguiste
            ya no es válido. Verificá la dirección o volvé al inicio.
          </p>

          <button
            type="button"
            onClick={handleGoHome}
            className="mt-9 inline-flex items-center gap-2 rounded-full bg-white px-8 py-3 text-sm font-semibold text-[#1e3a5f] shadow-lg transition-all duration-200 hover:-translate-y-0.5 hover:bg-blue-50 hover:shadow-xl focus:outline-none focus:ring-2 focus:ring-white/70 focus:ring-offset-2 focus:ring-offset-[#1e3a5f]"
          >
            <i className="pi pi-home" />
            Volver al inicio
          </button>
        </div>
      </div>
    </div>
  );
}
