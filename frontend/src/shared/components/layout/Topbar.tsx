import { useState, useRef, useEffect } from "react";
import { useAuth } from "@/app/providers/AuthProvider";
import { useNavigate } from "react-router-dom";
import { useResponsive } from "../../hooks/useResponsive";

export default function Topbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const { isMobile } = useResponsive();

  const isAdmin = user?.role_id === 1;

  // ================= STATE =================
  const [openUser, setOpenUser] = useState(false);
  const [openSettings, setOpenSettings] = useState(false);
  const [openMenu, setOpenMenu] = useState<string | null>(null);
  const [openMobileMenu, setOpenMobileMenu] = useState(false);

  // ================= REFS =================
  const userRef = useRef<HTMLDivElement>(null);
  const settingsRef = useRef<HTMLDivElement>(null);
  const menuRef = useRef<HTMLDivElement>(null);

  // ================= DERIVED =================
  const initials =
    user?.user_name
      ?.split(" ")
      .map((n: string) => n[0])
      .join("")
      .toUpperCase() || "U";

  // ================= ACTIONS =================
  const handleLogout = () => {
    logout();
    setOpenUser(false);
    setOpenMenu(null);
    navigate("/login", { replace: true });
  };

  // ================= EFFECTS =================
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (userRef.current && !userRef.current.contains(e.target as Node)) {
        setOpenUser(false);
      }

      if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
        setOpenMenu(null);
        setOpenMobileMenu(false);
      }

      if (settingsRef.current && !settingsRef.current.contains(e.target as Node)) {
        setOpenSettings(false);
      }
    };

    const handleEsc = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        setOpenUser(false);
        setOpenMenu(null);
        setOpenMobileMenu(false);
        setOpenSettings(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    document.addEventListener("keydown", handleEsc);

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
      document.removeEventListener("keydown", handleEsc);
    };
  }, []);

  // ================= RENDER =================
  return (
    <div
      className={`h-14 bg-white shadow flex items-center justify-between pr-4 md:pr-6 transition-all duration-300 ${
        isMobile ? "pl-16" : "pl-6"
      }`}
    >
      {/* ================= LEFT ================= */}
      <div className="flex items-center gap-4 relative w-full" ref={menuRef}>
        
        {/* MOBILE MENU BUTTON */}
        {isMobile && (
          <button
            onClick={() => {
              setOpenMobileMenu(!openMobileMenu);
              setOpenUser(false);
            }}
            className="p-2 rounded-md hover:bg-gray-100 transition"
          >
            <i className="pi pi-bars text-lg" />
          </button>
        )}

        {/* DESKTOP MENU */}
        {!isMobile && (
          <button
            onClick={() =>
              setOpenMenu(
                openMenu === "carga_operacion_banco"
                  ? null
                  : "carga_operacion_banco"
              )
            }
            className="flex items-center gap-1 text-sm font-medium text-gray-700 hover:text-blue-600 transition"
          >
            Carga Operación Banco
            <i
              className={`pi pi-angle-down text-xs transition-transform ${
                openMenu === "carga_operacion_banco" ? "rotate-180" : ""
              }`}
            />
          </button>
        )}

        {/* MOBILE DROPDOWN */}
        {isMobile && openMobileMenu && (
          <div className="absolute top-full left-0 mt-2 min-w-[180px] max-w-[260px] bg-white shadow-lg rounded-lg border z-50 py-2 animate-fadeIn divide-y">
            
            <button
              onClick={() =>
                setOpenMenu(
                  openMenu === "carga_operacion_banco"
                    ? null
                    : "carga_operacion_banco"
                )
              }
              className="flex justify-between items-center px-4 py-3 text-sm hover:bg-gray-100"
            >
              Carga Operación Banco
              <i className="pi pi-angle-down text-xs" />
            </button>

            {openMenu === "carga_operacion_banco" && (
              <button
                onClick={() => {
                  navigate("/home/carga_operacion_banco");
                  setOpenMenu(null);
                  setOpenMobileMenu(false);
                }}
                className="w-full text-left px-6 py-3 text-sm hover:bg-gray-100"
              >
                Nueva
              </button>
            )}
          </div>
        )}

        {/* DESKTOP DROPDOWN */}
        {!isMobile && openMenu === "carga_operacion_banco" && (
          <div className="absolute top-full mt-2 z-50 bg-white shadow-lg rounded-lg py-2 border left-0 min-w-[180px]">
            <button
              onClick={() => {
                navigate("/home/carga_operacion_banco");
                setOpenMenu(null);
              }}
              className="w-full text-left px-4 py-2 hover:bg-gray-100 text-sm"
            >
              Nueva
            </button>
          </div>
        )}
      </div>

      {/* ================= RIGHT ================= */}
      <div className="flex items-center gap-4">

        {/* SETTINGS (ADMIN ONLY) */}
        {isAdmin && (
          <div className="relative" ref={settingsRef}>
            
            <div
              onClick={() => {
                setOpenSettings(!openSettings);
                setOpenUser(false);
              }}
              className="cursor-pointer hover:bg-gray-100 p-2 rounded-md transition"
            >
              <i
                className={`pi pi-cog text-lg transition-transform ${
                  openSettings ? "rotate-180" : ""
                }`}
              />
            </div>

            {openSettings && (
              <div className="absolute right-0 mt-2 w-56 bg-white rounded-lg shadow-lg border z-50 py-2 animate-fadeIn">
                
                <div className="px-4 py-2 border-b text-sm font-semibold text-gray-700">
                  Administración
                </div>

                <button
                  onClick={() => {
                    navigate("/home/administracion_usuarios");
                    setOpenSettings(false);
                  }}
                  className="w-full text-left px-4 py-2 text-sm hover:bg-gray-100"
                >
                  <i className="pi pi-users mr-2" />
                  Usuarios
                </button>
                <button
                  onClick={() => {
                    navigate("/home/administracion_roles");
                    setOpenSettings(false);
                  }}
                  className="w-full text-left px-4 py-2 text-sm hover:bg-gray-100"
                >
                  <i className="pi pi-id-card mr-2" />
                  Roles
                </button>
                <button
                  onClick={() => {
                    navigate("/home/reasignacion-desestimacion");
                    setOpenSettings(false);
                  }}
                  className="w-full text-left px-4 py-2 text-sm hover:bg-gray-100"
                >
                  <i className="pi pi-refresh mr-2" />
                  Reasignar y Desestimar Actividades
                </button>
              </div>
            )}
          </div>
        )}

        {/* USER */}
        <div className="relative" ref={userRef}>
          <div
            onClick={() => setOpenUser(!openUser)}
            className="flex items-center gap-2 cursor-pointer hover:bg-gray-100 px-2 py-1 rounded-md transition"
          >
            <div className="w-9 h-9 rounded-full bg-blue-600 text-white flex items-center justify-center text-sm font-semibold">
              {initials}
            </div>

            <div className="hidden md:flex flex-col leading-tight">
              <span className="text-sm font-semibold text-gray-800 truncate">
                {user?.user_name || "Usuario"}
              </span>
              <span className="text-xs text-gray-500 truncate">
                {user?.role_name || "Rol"}
              </span>
            </div>

            <i className="pi pi-angle-down text-xs ml-auto" />
          </div>

          {openUser && (
            <div className="absolute right-0 mt-2 w-56 bg-white rounded-lg shadow-lg border z-50 py-2 animate-fadeIn">
              
              <div className="px-4 py-3 border-b">
                <p className="text-sm font-semibold">
                  {user?.user_name}
                </p>
                <p className="text-xs text-gray-500">
                  {user?.role_name}
                </p>
              </div>

              <button
                onClick={handleLogout}
                className="w-full text-left px-4 py-2 text-sm hover:bg-gray-100 text-red-600"
              >
                <i className="pi pi-sign-out mr-2" />
                Cerrar Sesión
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}