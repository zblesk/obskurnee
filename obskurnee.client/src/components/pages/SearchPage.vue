<template>
    <section class="book">
        <h1 class="page-title">{{$t('menus.search')}}</h1>
        <div class="form-field">
            <input v-model="searchTerm" v-on:keydown="doSearch" id="searchTerm" required :placeholder="$t('search.startSearching')" />
        </div>
        <span v-if="resultCount == 0"></span>
        <span v-else>{{ $t('search.resultCount', [ resultCount ]) }}</span>
        <p v-if="resultCount == 0">
            {{ $t('search.howTo') }}
        </p>
        <p v-else>
            <div class="grid">
                <template v-for="result in searchResults"
                          v-bind:key="result.postId"
                          v-bind:result="result">
                    <search-result-card :result="result" />
                </template>
            </div>
        </p>
    </section>
</template>

<style scoped>
    .books-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
        gap: var(--spacer);
        padding: var(--spacer);
    }

    .placeholder {
        text-align: center;
        font-style: italic;
    }
</style>

<script>
    import axios from "axios";
    import SearchResultCard from "../SearchResultCard.vue";
    export default {
        name: "Search",
        data() {
            return {
                searchTerm: "",
                searchResults: [],
            }
        },
        components: { SearchResultCard },
        computed: {
            resultCount() { return (this.searchResults?.length) ?? 0 }
        },
        methods: {
            async doSearch(e) {
                if (e.key.length != 1)
                    return;
                let newterm = this.searchTerm + e.key;
                if (newterm.length < 3) {
                    this.notice = this.$t('search.startSearching')
                    this.searchResults = []
                    return
                }
                this.searchResults = (await axios
                    .get('/api/search?query=' + newterm))
                    .data

            }
        },
        async mounted() {
        }
    }
</script>