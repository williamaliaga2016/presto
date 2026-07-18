namespace Multibanca.DTO.Utilidades
{
    public class UtilidadesDTO
    {
        public long id_expediente { get; set; }
        public int accion_id { get; set; }
        public long actividad_id { get; set; }
        public int actividad_usuario_id { get; set; }
        public string observaciones { get; set; } = string.Empty;

        /******** UTILIDAD EXCEPCIONES ************/
        public string excepcion_id { get; set; } = string.Empty;
        public string accion_excepcion_id { get; set; } = string.Empty;
        public string usuario_excepcion_id { get; set; } = string.Empty;
        public int user_id { get; set; }
    }
}