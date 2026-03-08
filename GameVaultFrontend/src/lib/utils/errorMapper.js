/**
 * Преобразует технические ошибки сервера в понятные сообщения на русском языке.
 * @param {any} error - Объект ошибки (Axios error или строка)
 * @param {string|null} tableId - ID текущей таблицы для контекстных сообщений
 * @returns {string} Локализованное сообщение об ошибке
 */
export const mapError = (error, tableId = null) => {
  if (!error) return 'Произошла ошибка';
  
  const rawMessage = typeof error === 'string' ? error : (error?.response?.data || error?.message || '');
  const message = String(rawMessage);

  // Foreign Key Constraints
  if (message.includes('foreign key constraint fails')) {
    // 1. Specific constraint checks (Creation/Update)
    if (message.includes('FK_Publishers_Country')) {
      return 'Указанная страна не найдена. Пожалуйста, выбери существующую страну или сначала добавь её в раздел "Страны".';
    }
    if (message.includes('FK_Games_Publisher')) {
      return 'Указанный издатель не найден. Пожалуйста, убедитесь, что издатель существует в системе (раздел "Издатели").';
    }

    // 2. Generic deletion check (No Cascade Delete / RESTRICT)
    if (message.includes('REFERENCES') || message.includes('a child row')) {
      return 'Невозможно удалить эту запись, так как она используется в других таблицах. Сначала удалите связанные с ней данные.';
    }

    return 'Нарушена целостность данных: некоторые связанные записи не существуют.';
  }

  // Unique Constraints / Duplicate Entries
  if (message.includes('Duplicate entry')) {
    if (message.includes('PRIMARY') && tableId === 'players') {
      return 'Игрок с таким никнеймом уже зарегистрирован.';
    }
    if (message.includes('email') || message.includes('Email')) {
      return 'Этот Email уже используется другим игроком.';
    }
    return 'Такая запись уже существует.';
  }

  // Data Length
  if (message.includes('Data too long') || message.includes('too long')) {
    return 'Введенный текст слишком длинный.';
  }

  // Common Network / Server Errors
  if (message.includes('Network Error')) {
    return 'Ошибка сети. Проверьте подключение к интернету или статус сервера.';
  }

  if (message.includes('404')) {
    return 'Запрашиваемый ресурс не найден.';
  }

  // Simple cleanup
  return message.replace('Ошибка сервера: ', '').trim() || 'Произошла ошибка при обработке запроса';
};
