import { useNavigate, useLocation } from "react-router-dom";
import { useState, useEffect } from "react";
import { useResponsive } from "../../hooks/useResponsive";

export default function Sidebar() {
  const navigate = useNavigate();
  const location = useLocation();

  const { isMobile } = useResponsive();

  const [openMobile, setOpenMobile] = useState(false);

  const [collapsed, setCollapsed] = useState<boolean>(() => {
    return JSON.parse(localStorage.getItem("sidebar-collapsed") || "false");
  });

  useEffect(() => {
    if (isMobile) {
      setCollapsed(false);
      setOpenMobile(false);
    } else {
      const saved = JSON.parse(
        localStorage.getItem("sidebar-collapsed") || "false"
      );
      setCollapsed(saved);
    }
  }, [isMobile]);

  useEffect(() => {
    if (!isMobile) {
      localStorage.setItem("sidebar-collapsed", JSON.stringify(collapsed));
    }
  }, [collapsed, isMobile]);

  const items = [
    { label: "Bandeja de Actividades", icon: "pi pi-home", path: "/home/bandeja" },
    { label: "Reportes", icon: "pi pi-chart-bar", path: "/home/reportes" },
    { label: "Consulta de Actividades", icon: "pi pi-search", path: "/home/consulta_actividades" },
    // { label: "Dashboard BI", icon: "pi pi-chart-line", path: "/home/dashboardBI" },
    { label: "Validar Información", icon: "pi pi-check-circle", path: "/home/validar_informacion" },
    { label: "Asignar Firmas, Peritos y Abogados", icon: "pi pi-users", path: "/home/asignar_firmas" },
  ];

  return (
    <>
      {/* BOTÓN HAMBURGUESA SOLO EN MOBILE */}
      {isMobile && (
        <button
          onClick={() => setOpenMobile(!openMobile)}
          className="fixed top-3 left-3 z-50 bg-[#03298e] text-white 
                     w-10 h-10 rounded-lg shadow-lg flex items-center justify-center"
        >
          <i className="pi pi-bars text-lg"></i>
        </button>
      )}

      {/* FONDO OSCURO */}
      {isMobile && openMobile && (
        <div
          onClick={() => setOpenMobile(false)}
          className="fixed inset-0 bg-black/40 z-40"
        />
      )}

      {/* SIDEBAR */}
      <div
        className={`fixed md:relative z-50 h-screen flex flex-col 
        bg-[#03298e] text-white shadow-2xl transition-all duration-300
        ${
          isMobile
            ? openMobile
              ? "w-64 left-0"
              : "w-64 -left-64"
            : collapsed
            ? "w-20"
            : "w-64"
        }`}
      >
        {/* LOGO */}
        <div className="h-14 flex items-center justify-center bg-white shadow">
          <img
            src={collapsed && !isMobile ? "/IconoCG.png" : "/LogoCG.png"}
            alt="Logo"
            className="h-8 object-contain"
          />
        </div>

        {/* MENÚ */}
        <div className="flex-1 p-3 space-y-2 overflow-y-auto">
          {items.map((item) => {
            const active =
              item.path === "/home"
                ? location.pathname === "/home"
                : location.pathname.startsWith(item.path);

            return (
              <div
                key={item.path}
                onClick={() => {
                  navigate(item.path);
                  if (isMobile) setOpenMobile(false);
                }}
                className={`flex items-center p-3 rounded-lg cursor-pointer 
                transition-all duration-200
                ${collapsed && !isMobile ? "justify-center" : "gap-3"}
                ${
                  active
                    ? "bg-white text-[#03298e] shadow-md"
                    : "hover:bg-white/20"
                }`}
              >
                <i
                  className={`${item.icon} text-lg w-5 min-w-[20px] text-center flex-shrink-0 ${
                    active ? "text-[#03298e]" : "text-white"
                  }`}
                />

                {!collapsed || isMobile ? (
                  <span className="font-medium whitespace-nowrap ml-2">
                    {item.label}
                  </span>
                ) : null}
              </div>
            );
          })}
        </div>

        {/* BOTÓN COLAPSAR SOLO DESKTOP */}
        {!isMobile && (
          <button
            onClick={() => setCollapsed(!collapsed)}
            className="absolute -right-3 top-1/2 -translate-y-1/2 
                       bg-white text-[#03298e] shadow-lg 
                       w-8 h-8 flex items-center justify-center 
                       rounded-full hover:scale-110 transition-all"
          >
            <i
              className={`pi ${
                collapsed ? "pi-angle-right" : "pi-angle-left"
              } text-xs`}
            ></i>
          </button>
        )}
      </div>
    </>
  );
}
