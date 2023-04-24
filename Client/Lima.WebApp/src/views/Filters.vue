<template>
  <div class="filters">
    <div class="filters-container">
      <div class="filter">
        <div class="filter-left">
          <i class="icofont-chart-bar-graph"></i>
          <div class="filter-name">{{ $t("filter_settings") }}</div>
        </div>
      </div>
      <div class="wrapper">
        <div class="filters-box">
          <div class="filters-box__title">{{ $t("reserve") }}</div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("number") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("pharmacy") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("date") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("amount_of_prepayment_from") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("check_number") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("staff") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("wholesalers") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("total_amount") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("amount_of_sum_left") }}</p></label
              >
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("intr") }}</p></label
              >
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">{{ $t("remains") }}</div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("number") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("pharmacy") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("date") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("amount_of_prepayment_from") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("check_number") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("staff") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("wholesalers") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("total_amount") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("amount_of_sum_left") }}</p></label
              >
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("intr") }}</p></label
              >
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">{{ $t("pharmacies") }}</div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("title_p") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("intr") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("amount_of_prepayment_from") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("total_booking") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("regions") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("total_amount") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("total_sales") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("amount_of_debt_with") }}</p>
              </label>
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">{{ $t("wholesalers") }}</div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("title_p") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("intr") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("amount_of_prepayment_from") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("total_booking") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("regions") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("total_amount") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("total_sales") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("amount_of_debt_with") }}</p>
              </label>
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">
            {{ $t("visits_to_the_pharmacy") }}
          </div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("title_p") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("date") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("amount") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("status") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("employee") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("time") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("prepayment") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("intr") }}</p>
              </label>
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">
            {{ $t("visit_to_the_wholesaler") }}
          </div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("title") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("date") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("the_amount") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("status") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("employee") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("time") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("prepayment") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("intr") }}</p>
              </label>
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">{{ $t("events") }}</div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("employee") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("time") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("date") }}</p>
              </label>
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">{{ $t("doctors") }}</div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("lfm") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("medical_institution") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("telephone") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("specialization") }}</p>
              </label>
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">{{ $t("medical_institution") }}</div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("title_p") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("type") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("doctor") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("intr") }}</p>
              </label>
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">{{ $t("map") }}</div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("employee") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("date") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("doctor") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("regions") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("organization") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("time") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("product") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("regions") }}</p>
              </label>
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">{{ $t("visit_doctors") }}</div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("employee") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("doctor") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("specialization") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("date") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("product") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("medical_institution") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("telephone") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("status") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("time") }}</p>
              </label>
            </div>
          </div>
        </div>
        <div class="filters-box">
          <div class="filters-box__title">{{ $t("products") }}</div>
          <div class="filters-inner">
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("title_p") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("a_country") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("expiration_date") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("quantity_short") }}</p>
              </label>
            </div>
            <div class="filters-inner__box">
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("manufacturer") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("warehouse") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("umit_measurement") }}</p>
              </label>
              <label class="login-form__container">
                <input type="checkbox" class="login-form__checkbox" />
                <span class="login-form__checkmark"></span>
                <p>{{ $t("reservations") }}</p>
              </label>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {};
</script>

<style></style>
