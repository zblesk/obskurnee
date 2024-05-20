<template>
    <section>
        <div class="main">
            <h1 class="page-title">{{$t('menus.search')}}</h1>
            <div class="form-field">
                <input v-model="searchTerm" v-on:keydown="doSearch" id="searchTerm" required :placeholder="$t('search.startSearching')" />
            </div>
            <div class="linefields">
                <input v-model="searchPosts" type="checkbox" id="searchPosts" required />
                <label for="searchPosts">{{ $t('search.searchPosts') }}</label>
                <input v-model="searchRecs" type="checkbox" id="searchRecs" required />
                <label for="searchRecs">{{ $t('search.searchRecs') }}</label>
            </div>
            <p>
                <span v-if="resultCount == 0">
                    {{ $t('search.howTo') }}
                </span>
                <span v-else>{{ $t('search.resultCount', [ resultCount ]) }}</span>
            </p>
        </div>
        <p>
            <div class="grid">
                <template v-for="result in filteredResults"
                          v-bind:key="result.postId"
                          v-bind:result="result">
                    <search-result-card :result="result" />
                </template>
            </div>
        </p>
    </section>
</template>

<script>
    import axios from "axios";
    import SearchResultCard from "../SearchResultCard.vue";
    export default {
        name: "Search",
        data() {
            return {
                searchTerm: "",
                searchResults: [],
                searchPosts: true,
                searchRecs: true
            }
        },
        components: { SearchResultCard },
        computed: {
            resultCount() { return (this.searchResults?.length) ?? 0 },
            filteredResults() {
                let wanted = [];
                if (this.searchPosts)
                    wanted.push('Post');
                if (this.searchRecs)
                    wanted.push('Rec');
                return this.searchResults.filter(r => wanted.includes(r.kind));
            }
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

<style scoped>
    .linefields {
        margin-bottom: var(--spacer);
        align-content: center;
        margin-left: 1em;
    }

    .linefields label {
        font-size: 1rem;
        margin-bottom: calc(var(--spacer) / 3);
        padding-left: 1ex;
        padding-right: 1em;
    }

    .linefields input {
    }
</style>