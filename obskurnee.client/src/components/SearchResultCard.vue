<template>
    <search-result-book v-if="result.kind == 'Post'" :post="result"></search-result-book>
    <search-result-recommendation v-if="result.kind == 'Rec'" :recommendation="result"></search-result-recommendation>
</template>

<script>
import SearchResultRecommendation from './SearchResultRecommendation.vue';
import SearchResultBook from './SearchResultBook.vue';
export default {
  components: { SearchResultRecommendation, SearchResultBook },
  name: "SearchResultCard",
  props: {
    result: {
      type: Object,
      required: true,
    }
  }
}
</script>