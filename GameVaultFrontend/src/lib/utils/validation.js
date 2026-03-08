/**
 * Validation utility for form inputs
 */

export const validationRules = {
  // Common text field validation (no Cyrillic, no special chars, no pure digits)
  text: (value) => {
    if (!value) return 'Поле не может быть пустым';
    
    // Check for Cyrillic letters
    if (/[а-яё]/i.test(value)) {
      return 'Использование русских букв запрещено';
    }
    
    // Check for special characters: &^$%@#№!
    if (/[&^$%@#№!]/.test(value)) {
      return 'Специальные символы (&^$%@#№!) запрещены';
    }
    
    // Check if it's only digits
    if (/^\d+$/.test(value)) {
      return 'Поле не может состоять только из цифр';
    }
    
    return null;
  },

  // Country specific (no digits allowed)
  country: (value) => {
    if (!value) return 'Поле не может быть пустым';
    
    // Check for Cyrillic letters
    if (/[а-яё]/i.test(value)) {
      return 'Использование русских букв запрещено';
    }
    
    // Check for special characters
    if (/[&^$%@#№!]/.test(value)) {
      return 'Специальные символы запрещены';
    }
    
    // Check for digits (prohibited for countries)
    if (/\d/.test(value)) {
      return 'Название страны не может содержать цифры';
    }
    
    return null;
  },

  // Date validation (must be between 2026 and 2030)
  date: (value) => {
    if (!value) return 'Дата не выбрана';
    
    const year = new Date(value).getFullYear();
    const currentYear = new Date().getFullYear();
    const maxYear = currentYear + 5;
    if (year < 1985 || year > maxYear) {
      return `Год должен быть в диапазоне от 1985 до ${maxYear}`;
    }
    
    return null;
  },

  // Positive Number validation (for weight)
  positiveNumber: (value) => {
    if (value === null || value === undefined || value === '') return 'Поле не может быть пустым';
    const num = parseFloat(value);
    if (isNaN(num) || num <= 0) {
      return 'Значение должно быть положительным числом';
    }
    return null;
  }
};

/**
 * Normalizes a string (Trims and Capitalizes First Letter, lowers the rest)
 */
export const normalize = (value) => {
  if (!value) return '';
  const trimmed = value.trim();
  if (!trimmed) return '';
  
  return trimmed
    .split(/\s+/)
    .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
    .join(' ');
};

/**
 * Validates a form object against rules
 * @param {Object} data - Form data
 * @param {Object} schema - Mapping of field names to rule types (text, date, etc)
 * @returns {string|null} - First error message or null
 */
export const validateForm = (data, schema) => {
  for (const [field, ruleKey] of Object.entries(schema)) {
    const error = validationRules[ruleKey](data[field]);
    if (error) return error;
  }
  return null;
};
