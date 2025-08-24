
//criacao playload da requisicao como "record"

public  record ProductRequestDTO(string Code, string Name, int CategoryId, List<string> Tags);
