namespace DeviceManagement.Application.Common;

public static class ErrorMessages
{
    // Mensagens de entidades não encontradas
    public static class NotFound
    {
        public const string Cliente = "O cliente solicitado não foi encontrado no sistema. Verifique se o ID está correto.";
        public const string Dispositivo = "O dispositivo solicitado não foi encontrado no sistema. Verifique se o ID está correto.";
        public const string Evento = "O evento solicitado não foi encontrado no sistema. Verifique se o ID está correto.";
    }    // Mensagens de validação de negócio
    public static class BusinessRules
    {
        public const string ClienteComDispositivosAtivos = "Não é possível desativar o cliente pois ele possui dispositivos ativos. Desative todos os dispositivos primeiro.";
        public const string ClienteComDispositivos = "Não é possível excluir o cliente pois ele possui dispositivos vinculados. Remova todos os dispositivos primeiro.";
        public const string DispositivoComEventos = "Não é possível excluir o dispositivo pois ele possui eventos vinculados. Remova todos os eventos primeiro.";
        public const string ClienteInativo = "Não é possível adicionar dispositivo a um cliente inativo. Ative o cliente primeiro.";
        public const string DispositivoInativo = "Não é possível registrar eventos em um dispositivo inativo. Ative o dispositivo primeiro.";
        public const string SerialJaExiste = "Já existe um dispositivo com este número de série para este cliente. Use um número de série diferente.";
        public const string SerialJaExisteOutroDispositivo = "Este número de série já está em uso por outro dispositivo no sistema. Use um número de série diferente.";
        public const string EmailJaExiste = "Já existe um cliente cadastrado com este endereço de email. Use um email diferente.";
        public const string DataFutura = "A data informada não pode ser no futuro. Verifique a data e tente novamente.";
        public const string DataInicialMaiorQueFinal = "A data inicial deve ser anterior à data final. Verifique o período informado.";
    }

    // Mensagens de erro interno
    public static class Internal
    {
        public const string DatabaseError = "Ocorreu um erro interno no banco de dados. Tente novamente em alguns momentos.";
        public const string UnexpectedError = "Ocorreu um erro inesperado no sistema. Entre em contato com o suporte técnico.";

        public static string CreateEntity(string entityName) =>
            $"Erro interno ao criar {entityName}. Tente novamente ou entre em contato com o suporte.";

        public static string UpdateEntity(string entityName) =>
            $"Erro interno ao atualizar {entityName}. Tente novamente ou entre em contato com o suporte.";

        public static string DeleteEntity(string entityName) =>
            $"Erro interno ao excluir {entityName}. Tente novamente ou entre em contato com o suporte.";

        public static string GetEntity(string entityName) =>
            $"Erro interno ao buscar {entityName}. Tente novamente ou entre em contato com o suporte.";
    }    // Mensagens de validação de entrada
    public static class Validation
    {
        public const string InvalidEmailFormat = "O formato do email é inválido. Use um email válido como exemplo@dominio.com";
        public const string RequiredField = "Este campo é obrigatório e deve ser preenchido.";
        public const string InvalidImeiFormat = "O IMEI deve conter exatamente 15 dígitos numéricos.";
        public const string InvalidImeiChecksum = "O IMEI informado possui dígito verificador inválido. Verifique se o número está correto.";
        public const string InvalidSerialLength = "O número de série deve ter entre 1 e 100 caracteres.";
        public const string InvalidNameLength = "O nome deve ter entre 1 e 200 caracteres.";
        public const string InvalidPhoneLength = "O telefone deve ter no máximo 20 caracteres.";
        public const string InvalidEventType = "O tipo de evento informado é inválido. Use um dos tipos válidos: Ligado, Desligado, Movimento, QuedaSinal.";
        public const string EmptyEmail = "O endereço de email é obrigatório e não pode estar vazio.";
        public const string EmptyImei = "O IMEI é obrigatório e não pode estar vazio.";
        public const string EmptySerial = "O número de série é obrigatório e não pode estar vazio.";
        public const string EmptyName = "O nome é obrigatório e não pode estar vazio.";
        public const string DeviceAlreadyActive = "O dispositivo já está ativo. Não é necessário ativá-lo novamente.";
    }

    // Mensagens de autenticação
    public static class Authentication
    {
        public const string InvalidCredentials = "Credenciais inválidas. Verifique o nome de usuário e senha.";
        public const string TokenExpired = "Seu token de acesso expirou. Faça login novamente.";
        public const string TokenInvalid = "Token de acesso inválido. Faça login novamente.";
        public const string Unauthorized = "Você não tem permissão para acessar este recurso.";
    }
}
