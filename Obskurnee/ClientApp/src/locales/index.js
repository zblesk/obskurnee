import { createI18n } from 'vue-i18n';
import sk from "./sk.js";
import en from "./en.js";

const messages = {
  sk: sk,
  en: en
};

export default createI18n({
  locale: 'sk', 
  fallbackLocale: 'sk', 
  messages, 
});