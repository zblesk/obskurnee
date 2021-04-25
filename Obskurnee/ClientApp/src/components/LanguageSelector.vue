<template>
    <div class="form-horizontal">
        <label for="language-select">Vyber preferovaný jazyk</label>
        <select v-model="$i18n.locale" id="language-select">
            <option v-for="locale in $i18n.availableLocales" :key="`locale-${locale}`" :value="locale">{{ locale }}</option>
        </select>
    </div>
</template>

<style scoped>
    .form-horizontal select {
        width: 100px;
    }
</style>