import { Outlet } from "react-router-dom";
import { useAuth } from "@/app/providers/AuthProvider";
import Menu from "../../../shared/components/layout/Menu.tsx";
import Topbar from "../../../shared/components/layout/Topbar.tsx";

/**
 * Renderiza el contenedor protegido y oculta la navegacion global para sesiones temporales.
 *
 * @returns Layout principal para rutas bajo `/home`.
 */
export default function Home() {
  const { isTemporalAccess } = useAuth();

  return (
    <div className="h-screen flex bg-gray-200">

      {/* SIDEBAR */}
      {!isTemporalAccess && <Menu />}

      {/* CONTENIDO */}
      <div className="flex-1 flex flex-col min-w-0">

        {/* TOPBAR */}
        {!isTemporalAccess && <Topbar />}

        {/* MAIN */}
        <div className="flex-1 overflow-auto p-2 sm:p-6 min-w-0">

          <div className="bg-white rounded-xl shadow-lg p-3 sm:p-6 w-full min-w-0">
            <Outlet />
          </div>

          <p className="text-center text-xs text-gray-400 mt-6">
            © {new Date().getFullYear()} Multibanca - Todos los derechos reservados
          </p>

        </div>
      </div>
    </div>
  );
}
