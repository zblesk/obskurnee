<template>
    <div class="form-horizontal">
        <label for="language-select">Vyber preferovaný jazyk</label>
        <select v-model="$i18n.locale" id="language-select">
            <option v-for="locale in $i18n.availableLocales" :key="`locale-${locale}`" :value="locale">{{ locale }}</option>
        </select>
    </div>
</template>

<script>
import { mapActions } from 'vuex';
export default {
  name: 'LanguageSelector',
  watch: {
    '$i18n.locale' (to) {
      this.updateLanguage(to)
        .then(() => this.$notifySuccess(this.$t('messages.langChangeSuccess')))
        .catch(this.$handleApiError);
    }
  },
  methods: {
      ...mapActions("users", ["updateLanguage"]),
  }
}
</script>

<style scoped>
    .form-horizontal select {
        width: 100px;
    }
</style>